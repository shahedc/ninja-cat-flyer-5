using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour {
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;

    public Transform groundCheckTransform;
    private bool grounded;
    public LayerMask groundCheckLayerMask;
    Animator animator;

    public ParticleSystem jetpack;

    private bool dead = false;

    private uint gems = 0;

    public Texture2D gemIconTexture;

    public AudioSource jetpackAudio;

    public AudioSource footstepsAudio;

    public AudioClip gemCollectSound;

    void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    {
        footstepsAudio.enabled = !dead && grounded;

        jetpackAudio.enabled = !dead && !grounded;
        jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;
    }

    void OnGUI()
    {
        DisplayGemsCount();

        DisplayRestartButton();
    }

    void DisplayGemsCount()
    {
        Rect gemIconRect = new Rect(10, 10, 32, 32);
        GUI.DrawTexture(gemIconRect, gemIconTexture);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        Rect labelRect = new Rect(gemIconRect.xMax, gemIconRect.y, 60, 32);
        GUI.Label(labelRect, gems.ToString(), style);
    }

    void CollectGem(Collider2D gemCollider)
    {
        gems++;

        Destroy(gemCollider.gameObject);

        AudioSource.PlayClipAtPoint(gemCollectSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Gems"))
            CollectGem(collider);
        else
            HitByLaser(collider);
    }
 
    void HitByLaser(Collider2D laserCollider)
    {
        if (!dead)
            laserCollider.gameObject.GetComponent<AudioSource>().Play();

        dead = true;

        animator.SetBool("dead", true);
    }

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");

        jetpackActive = jetpackActive && !dead;

        if (jetpackActive)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jetpackForce));
        }

        if (!dead)
        {
            Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
            newVelocity.x = forwardMovementSpeed;
            GetComponent<Rigidbody2D>().velocity = newVelocity;
        }

        UpdateGroundedStatus();

        AdjustJetpack(jetpackActive);

        AdjustFootstepsAndJetpackSound(jetpackActive);
    }

    void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        animator.SetBool("grounded", grounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        jetpack.enableEmission = !grounded;
        jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f;
    }

    void DisplayRestartButton()
    {
        if (dead && grounded)
        {
            Rect buttonRect = new Rect(Screen.width * 0.35f, Screen.height * 0.45f, Screen.width * 0.30f, Screen.height * 0.1f);
            if (GUI.Button(buttonRect, "Tap to restart!"))
            {
                Application.LoadLevel(Application.loadedLevelName);
            };
        }
    }
}
