using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed = 75f;
    public float jumpForce;
    public Rigidbody rig;
    public int health;

    public AudioSource source;
    public AudioClip oof;
    public AudioClip jump;

    public Animator anim;

    public GameObject playerObj;

    public int coinCount;
    void Move()
    {
        // get the input axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = playerObj.transform.forward * z + playerObj.transform.right * x;
        rig.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        Vector3 flatVel = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rig.velocity = new Vector3(limitedVel.x, rig.velocity.y, limitedVel.z);
        }

        //Vector3 rotation = Vector3.up * x;

        /*Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        //calculate a direction relative to where we are facing
        Vector3 dir = (transform.forward * z + transform.right * x) * moveSpeed;

        dir.y = rig.velocity.y;

        //set that as our velocity
         rig.velocity = dir;

        //rig.MoveRotation(rig.rotation * angleRot);*/

        //if we are moving play run animation otherwise play idle animation
        if(Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void TryJump()
    {
        // create a ray facing down
        Ray ray = new Ray(transform.position, Vector3.down);

        //shoot the raycast
        if (Physics.Raycast(ray, 1.5f)) {
            anim.SetTrigger("isJumping");
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            source.clip = jump;
            source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // input for movement
        Move();

        // input for jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }

        if(health <= 0)
        {
            anim.SetBool("die", true);
            StartCoroutine("DieButCool");
        }
    }
    IEnumerator DieButCool()
    {
        
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Enemy" || other.gameObject.name == "Cube")
        {
            health -= 50;
            source.clip = oof;
            source.Play();
        }

        if(other.gameObject.name == "FallCollider")
        {
            SceneManager.LoadScene(0);
        }
    }
}
