using UnityEngine;
using System.Collections;

public class InGameDecalTest : MonoBehaviour {

    public DecalType i_decalType;
    public DecalType i_blood;
    public DecalType i_flow;
    public GameObject i_bloodSplatter;
    public GameObject i_dust;
    public bool i_fluid;

    private bool _allowFire = true;

    //Update is called once per frame
    private void Start()
    {
        //Combine all uncombined Decals
        DecalCreator.CreateCombinedStaticDecalInGame();
        //Profiler.enabled = true;
    }
    private void Update()
    {
        Fire();
    }
    ///Delay between shoot
    private IEnumerator Delay()
    {
        _allowFire = false;
        yield return new WaitForSeconds(0.16F);
        _allowFire = true;
    }
    ///Shoot
    private void Fire()
    {
        if (Input.GetMouseButton(0) && _allowFire)
        {
            //Audio shoot
            //this.audio.PlayOneShot(this.audio.clip);

            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            bool wasHit = Physics.Raycast(ray, out hit);
            if (wasHit)
            {  
                //Profiler.BeginSample("BURN DECAL ON TORUS");
                //Burn decal
                Mesh decalMesh = DecalCreator.CreateDecalMesh(i_decalType, hit.point, -hit.normal, hit.collider.gameObject);
                //Create Decal Object
                DecalCreator.CreateDynamicDecal(decalMesh, hit.collider.gameObject, i_decalType);                 
                //Profiler.EndSample();

                //If we hit character
                if (hit.collider.transform.root.name == "Enemy")
                {
                    Blood(hit,ray.direction);
                    hit.collider.transform.root.SendMessage("ApplyDamage", SendMessageOptions.DontRequireReceiver);
                    SkinnedMeshRenderer smr = hit.collider.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
                    SkinDecal(hit, smr.gameObject);
                }
                else
                    Dust(hit);
            }

            StartCoroutine(Delay());
        }
    }
    ///Skin decal
    private void SkinDecal(RaycastHit hit, GameObject objWithSkin)
    {
        Mesh decalMesh=DecalCreator.CreateDecalMesh(i_blood, hit.point, -hit.normal, objWithSkin, Vector3.zero);
        DecalCreator.CreateDynamicSkinnedDecal(decalMesh, objWithSkin, i_blood, null);
    }
    ///Throw blood
    private void Blood(RaycastHit hit,Vector3 direction)
    {
        Instantiate(i_bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal)); 
       
    }
    ///Throw dust
    private void Dust(RaycastHit hit)
    {
        //Instantiate(i_dust, hit.point, Quaternion.LookRotation(hit.normal));
    }
}