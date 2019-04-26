using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class FogMenuItem{
    [MenuItem("GameObject/3D Object/Local Fog")]
    public static void init(){
        var fog = new GameObject("Local Fog");
        fog.AddComponent<LocalFog>();
    }
}

public enum Shape{
    Box,Sphere,Hemisphere
}
[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
public class LocalFog : MonoBehaviour
{
    [Header("Texture")]
    public Texture2D FogTexture;
    Texture2D m_FogTexture;
    public Material FogMat;
    Material m_FogMat;

    [Header("Particle System")]
    public int MaxParticle = 50;
    int m_MaxParticle;
    public int ParticleSize = 5;
    int m_ParticleSize;
    public Shape ParticleShape = Shape.Box;
    Shape m_ParticleShape = Shape.Sphere;
    public Vector3 ParticleShapeSize = new Vector3(10,1,10);
    Vector3 m_ParticleShapeSize;
    public bool HideParticleSystem = true;
    // [Private Properties]
    private ParticleSystem ParSys;
    void Reset(){
        transform.rotation = Quaternion.Euler(0,0,0);
    }
    void Update()
    {
        if(FogMat != null && FogTexture != null){
            if(ParSys == null){
                ParSys = GetComponent<ParticleSystem>();
                
            }
            if(HideParticleSystem){
                ParSys.hideFlags = HideFlags.HideInInspector;
            }else{
                ParSys.hideFlags = HideFlags.None;
            }
            if(m_FogTexture != FogTexture){
                FogMat.mainTexture = FogTexture;
                m_FogTexture = FogTexture;
                UpdateParticleSystem();
            }

            var main = ParSys.main;

            if(m_MaxParticle != MaxParticle){
                main.maxParticles = MaxParticle;
                main.prewarm = true;
                m_MaxParticle = MaxParticle;
                UpdateParticleSystem();
            }
            if(m_ParticleSize != ParticleSize){
                main.startSize = ParticleSize;
                m_ParticleSize = ParticleSize;
                UpdateParticleSystem();
            }
            
            main.startLifetime = 10000;
            main.startSpeed = 0;
                

            var em = ParSys.emission;
            em.enabled = true;
            em.rateOverTime = 100000;

            var rend = GetComponent<ParticleSystemRenderer>();
            rend.enabled = true;

            if(m_FogMat != FogMat){
                rend.material = FogMat;
                rend.trailMaterial = FogMat;
                m_FogMat = FogMat;
                UpdateParticleSystem();
            }

            var sh = ParSys.shape;
            sh.enabled = true;

            if(m_ParticleShape != ParticleShape){
                if(ParticleShape == Shape.Box)
                    sh.shapeType = ParticleSystemShapeType.Box;

                if(ParticleShape == Shape.Sphere)
                    sh.shapeType = ParticleSystemShapeType.Sphere;

                if(ParticleShape == Shape.Hemisphere)
                    sh.shapeType = ParticleSystemShapeType.Hemisphere;
                m_ParticleShape = ParticleShape;
                UpdateParticleSystem();
            }
            if(m_ParticleShapeSize != ParticleShapeSize){
                sh.scale = ParticleShapeSize;
                m_ParticleShapeSize = ParticleShapeSize;
                UpdateParticleSystem();
            }
        }
    }
    void UpdateParticleSystem(){
        ParSys.Simulate(0,true,true,true);
        ParSys.Play();
        
    }
}
