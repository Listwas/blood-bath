using System.Collections;
using UnityEngine;

public class PlayerImmunityAndKnockbackScript : MonoBehaviour
{
    [Header("Player immunity and knockback parameters")]
    [SerializeField] private float immunityDuration = 0.75f;
    //[SerializeField] private float knockbackDistance = 3f;
    [SerializeField] private float flashSpeed = 0.5f;
    private float knockbackDuration = 0.15f;

    private bool isImmune = false;
    private CombatScript combatScript;
    private Coroutine knockbackRoutine;
    private Renderer playerRenderer;


    private void Awake()
    {
        isImmune = false;
        combatScript = GetComponent<CombatScript>();
        playerRenderer = GetComponentInChildren<Renderer>();

        if (combatScript == null) Debug.LogWarning("Combat script missing!");
    }
    public void ReceiveHit(int damage, Transform attacker)
    {
        if (isImmune) return;

        if (combatScript != null)
            combatScript.TakeDamage(damage, attacker);

        StartCoroutine(ActivateImmunity());

        if (knockbackRoutine != null)
            StopCoroutine(knockbackRoutine);

        float knockbackDistance = damage * 0.15f;

        knockbackRoutine = StartCoroutine(KnockbackFrom(attacker.position, knockbackDistance));
    }

    private IEnumerator KnockbackFrom(Vector3 sourcePosition, float knockbackDistance)
    {
        Vector3 direction = (transform.position - sourcePosition).normalized;
        direction.y = 0f; // Ensure no vertical movement

        Vector3 start = transform.position;
        Vector3 end = start + direction * knockbackDistance;

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            float t = elapsed / knockbackDuration;
            transform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }

    private IEnumerator ActivateImmunity()
    {
        Debug.Log("Immunity frames started");
        isImmune = true;

        if (playerRenderer != null)
        {
            yield return StartCoroutine(FlashTemporaryMaterial(playerRenderer, immunityDuration, flashSpeed));
        }

        isImmune = false;
        Debug.Log("Immunity frames ended");
    }

    private IEnumerator FlashTemporaryMaterial(Renderer targetRenderer, float duration, float speed)
    {
        Material flashMaterial = new Material(Shader.Find("Unlit/Color"));
        flashMaterial.color = new Color(1f, 0f, 0f, 0f); // red, fully transparent

        flashMaterial.SetOverrideTag("RenderType", "Transparent");
        flashMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        flashMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        flashMaterial.SetInt("_ZWrite", 0);
        flashMaterial.DisableKeyword("_ALPHATEST_ON");
        flashMaterial.EnableKeyword("_ALPHABLEND_ON");
        flashMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        flashMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        Material[] originalMaterials = targetRenderer.materials;
        Material[] tempMaterials = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(tempMaterials, 0);
        tempMaterials[tempMaterials.Length - 1] = flashMaterial;

        targetRenderer.materials = tempMaterials;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.PingPong(Time.time * speed, 1f); // flash between 0 and 1
            Color c = flashMaterial.color;
            c.a = alpha;
            flashMaterial.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        targetRenderer.materials = originalMaterials;
        Destroy(flashMaterial);
    }
}

