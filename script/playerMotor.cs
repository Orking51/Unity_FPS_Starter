using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class playerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 rotation;
    private Vector3 velocity;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterVelocity;

    [SerializeField] 
    private float cameraRotationLimit = 85f;

    private Rigidbody rb;


    private void Start()
    {
        //permet de modifier/ de créé des valeur du rigidBody de perso 
        rb = GetComponent<Rigidbody>();
    }

    // _velocité c'est le nom de recuperation du vestor3 vélocité dans playerController
    public void Move(Vector3 _velocity)
    {
        //pour avoir une variable avec un nom simple on lui assigne le nom velocité pour cette page de code, le meme nom que on lui a donner lors de sa definition dans playerController
        velocity = _velocity;
    }

    // __rotation c'est le nom de recuperation du vestor3 rotation dans playerController
    public void Rotate(Vector3 _rotation)
    {
        //pour avoir une variable avec un nom simple on lui assigne le nom rotation pour cette page de code, le meme nom que on lui a donner lors de sa definition dans playerController
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotateX)
    {
        //pour avoir une variable avec un nom simple on lui assigne le nom rotation pour cette page de code, le meme nom que on lui a donner lors de sa definition dans playerController
        cameraRotationX = _cameraRotateX;
    }

    public void ApplyThruster(Vector3 _thrusterVelocity)
    {
        thrusterVelocity = _thrusterVelocity;
    }


    //petite difference avec la void update, celle ci est plus axés phisique, puisque que fixeupdate n'est pas dependant des frame/s mais d un tique regulié qui lui est propre 
    private void FixedUpdate()
    {
        //appelle une fonction qui effectue le mouvement de notre perso. 
        PerformMovement();
        PerformRotation();
    }

    // effectu le mouvement 
    private void PerformMovement()
    {
        
         
        //on verifie si il y a 1 deplacement ou non (0 = pas de mouvement) 
        if (velocity != Vector3.zero)
        {
            //regidBody(rb.) permet de faire le mouvement(de deplacé notre perso, c'est notre perso)
            //MovePosition, c'est dans le mot c pour deplacer la position
            //ce que on cherche a déplacer:
            // rb.position est sa position actuelle 
            // on ajoute la velocité au deplacement
            // pour que le rb ne se deplace pas brutalement on applique Time.fixDeltaTime ( fixed car on travaille avec de la physique et DeltaTime parce que on veut que ca s applique avec le temps )
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }


        
        //thrusterforce 
        if(thrusterVelocity != Vector3.zero) 
        { 
            rb.AddForce(thrusterVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        






    }
    // effectu la rotation de la cam et du perso 
    private void PerformRotation()
    {
        //calcule la rot de la cam
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        //on applique a la cam
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}