using UnityEngine;

public class BigExplosion
{
    public static int numberOfSmalls = 10;
    public static float maxRange = 25f;
    public static float minRange = 10f;

    public static GameObject GetExplosion()
    {
        var p = new GameObject();

        for (int i = 0; i < numberOfSmalls; ++i)
        {
            var e =
                SpawnManager.SharedInstance
                .GetInstance("Small Explosion Particle");
            e.transform.parent = p.transform;
            RequiredChanges(e);
            e.transform.localPosition = RandomDir();
        }

        return p;

    }

    static void RequiredChanges(GameObject e)
    {
        e.GetComponent<AudioSource>().volume = .25f;
    }

    static Vector3 RandomDir()
    {
        return new Vector3(
            Random.Range(minRange, maxRange),
            Random.Range(minRange, maxRange),
            Random.Range(minRange, maxRange)
        );
    }

}
