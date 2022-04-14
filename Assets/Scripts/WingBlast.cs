using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingBlast : MonoBehaviour
{
    [SerializeField] AudioClip wingBlastSound;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(wingBlastSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.SharedComponent.PlaySound(wingBlastSound);
            Shoot.blastMode = Shoot.WING_BLAST;
            var beams = SpawnManager.SharedInstance.GetAllInstances("Blast");
            for (int i = 0; i < beams.Count; ++i)
            {
                beams[i].GetComponent<Shoot>().SetModel(Shoot.WING_BLAST);
            }
            gameObject.SetActive(false);
        }
    }
}
