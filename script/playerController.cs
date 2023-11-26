    using UnityEngine;

// on ne peut supprimé playerMotor 
[RequireComponent(typeof(playerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]

public class playerController : MonoBehaviour
{

    // la variable de la vitesse du joueur, modifiable dans unity grace au parametre "[SerializeField]".
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float mouseSensitivityX = 3f;
    [SerializeField]
    private float mouseSensitivityY = 3f;
    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Joint Options")]
    [SerializeField] 
    private float JointYSpring = 20;
    [SerializeField]
    private float JointYMaxForce = 50;

    //on créé une reference a PlayerMotor qui s'appelle motor.
    private playerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    private void Start()
    {
        //playerMotor est dans motor
        motor = GetComponent<playerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSettings(JointYSpring);
    }

    private void Update()
    {

// calculer la velociter du joueur.

        // savoir si il va a droite ou a gauche.
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        // on assigne donc la demande du joueur (xMov, zMov) à une direction.
        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        // on assigne une vitesse (speed declaré comme vide) au mouvement.
        // le .normalized return une magnitude de 1  
        Vector3 velocity = (moveHorizontal + moveVertical) * speed;

        //jouer animation thruster 
        animator.SetFloat("ForwardVelocity", zMov);


        //appelle la fonction move et lui assigne la valeur de la velociter lors de l appelle
        motor.Move(velocity);


//rotation du jouer avec un vector 3

        //connaitre le regard de la souris
        float yRot = Input.GetAxisRaw("Mouse X");

        // on assigne donc la demande du joueur à une direction, rotation.
        Vector3 rotation = new Vector3 (0, yRot, 0) * mouseSensitivityX;

        motor.Rotate(rotation);

//rotation la cam avec un vector 3

        //demande du joueur
        float xRot = Input.GetAxisRaw("Mouse Y");

        // on assigne donc la demande du joueur à une direction, rotation.
        float cameraRotationX = xRot * mouseSensitivityY;

        motor.RotateCamera(cameraRotationX);

        
        //Jetpack
        Vector3 thrusterVelocity = Vector3.zero;
        //Applique la variable de saut 
        if (Input.GetButton("Jump"))
        {
         thrusterVelocity = Vector3.up * thrusterForce;
         SetJointSettings (0f); //desactivation de la gravité du vecteur
        }
        else
        {
         SetJointSettings(JointYSpring); //reactivation de la gravité du vecteur
        }

         motor.ApplyThruster(thrusterVelocity);

         
    }

    //parametre cofigurable joint prefabe
    private void SetJointSettings(float _JointYSpring)
    {
        joint.yDrive = new JointDrive { maximumForce = JointYMaxForce, positionSpring = _JointYSpring };  // reassigne les val a maximumForce et positionSpring

    }
}