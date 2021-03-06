﻿
using UnityEngine;
using UnityEngine.UI;

// ASP_Core_Damageable is a specialized version of ASP_Damageable just for the core
public class ASP_Core_Damageable : ASP_Damageable
{

    //Text output of current damage
    [SerializeField]
    private ASP_PercentAnimator damagePercentText;
    [SerializeField]
    private ASP_InnerCylinderAnimator innerCylinder;
    [SerializeField]

    private ASP_ParticleAnimator coreParticles;
    [SerializeField]

    private int VictoryPointPenaltyWhenDamaged = 1;
    protected override void Start()
    {
        //call base method of ASP_Core_Damageable
        base.Start();
        damagePercentText.UpdatePercent(100);
    }


    protected override void CauseDamage(int damage)
    {
        damageReceived += damage;
        ASP_GameManager.Instance.IncrementScore(-damage * VictoryPointPenaltyWhenDamaged );
        if (damageReceived >= hitPoints)
        {
            //destroyed
            innerCylinder.UpdateValue(0);
            damagePercentText.UpdatePercent(0);
            coreParticles.UpdateValue(0);
			ASP_GameManager.Instance.PlayerDamaged ((float)damageReceived/(float)hitPoints);
            ASP_GameManager.Instance.GameOver(GameResult.PlayerLossCore);
        }
        else
        {
            float newPercentage = 1 - ((float)damageReceived / (float)hitPoints);
            innerCylinder.UpdateValue(newPercentage);
            coreParticles.UpdateValue(newPercentage);
            damagePercentText.UpdatePercent(System.Convert.ToInt32(newPercentage * 100));
			ASP_GameManager.Instance.PlayerDamaged ((float)damageReceived/(float)hitPoints);
        }
    }
}
