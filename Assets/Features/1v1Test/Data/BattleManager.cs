using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures.Event;
using DataStructures.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public List<Unit> units;
    public ActionEventWithParameter<float> onTotalHealthChanged;
    public ScalingStats scalingStats;
    public MoneyReward moneyReward;
    public FloatVariable hostMoney;
    public FloatVariable guestMoney;
    public ClickerComponent clickerComponent;

    public TMP_Text stageText;
    public TMP_Text sharedMoneyText;
    public TMP_Text hostMoneyText;
    public TMP_Text guestMoneyText;

    private int stage;

    private void Awake()
    {
        foreach (Unit unit in units)
        {
            unit.attackDeltaTime = unit.entity.attackTime;
            unit.staminaDeltaTime = unit.entity.attackTime;
            unit.entity.currentStamina.Set(unit.entity.totalStamina.Get());
            
            unit.entity.currentStamina.RegisterOnValueChangedAction(delegate(int value)
            {
                UpdateStaminaSlider(value, unit);
            });
        }
        
        moneyReward.AddMoney(stage);
        
        stageText.text = "Stage: " + stage;
        sharedMoneyText.text = "Shared Money: " + moneyReward.sharedMoney.Get();
        guestMoneyText.text = "Host Money: " + guestMoney.Get();
        hostMoneyText.text = "Host Money: " + hostMoney.Get();
        
        onTotalHealthChanged.RegisterListener(UpdateHealth);
        moneyReward.sharedMoney.RegisterOnValueChangedAction(delegate(float currency)
        {
            UpdateText(currency, sharedMoneyText, "Shared Money: ");
        });
        
        hostMoney.RegisterOnValueChangedAction(delegate(float currency)
        {
            UpdateText(currency, hostMoneyText, "Host Money: ");
        });
        
        guestMoney.RegisterOnValueChangedAction(delegate(float currency)
        {
            UpdateText(currency, guestMoneyText, "Guest Money: ");
        });
    }

    private void UpdateStaminaSlider(int value, Unit unit)
    {
        unit.staminaSlider.value = (float)value / unit.entity.totalStamina.Get();
    }

    private void OnDestroy()
    {
        onTotalHealthChanged.UnregisterListener(UpdateHealth);
        
        moneyReward.sharedMoney.UnregisterOnValueChangedAction(delegate(float currency)
        {
            UpdateText(currency, sharedMoneyText, "Shared Money: ");
        });
        
        hostMoney.UnregisterOnValueChangedAction(delegate(float currency)
        {
            UpdateText(currency, hostMoneyText, "Host Money: ");
        });
        
        guestMoney.UnregisterOnValueChangedAction(delegate(float currency)
        {
            UpdateText(currency, guestMoneyText, "Guest Money: ");
        });
    }

    private void Update()
    {
        foreach (Unit unit in units)
        {
            unit.staminaDeltaTime -= Time.deltaTime;
            if (unit.staminaDeltaTime < 0)
            {
                unit.staminaDeltaTime = unit.entity.staminaRefreshTime;

                if (unit.entity.currentStamina.Get() < unit.entity.totalStamina.Get())
                {
                    unit.entity.currentStamina.Set(unit.entity.currentStamina.Get() + 1);
                    unit.staminaSlider.value = (float)unit.entity.currentStamina.Get() / unit.entity.totalStamina.Get();
                }
            }
            
            if (unit.entity.currentHealth.Get() > 0)
            {
                unit.attackDeltaTime -= Time.deltaTime;
                float sliderValue = unit.attackDeltaTime / unit.entity.attackTime;
                unit.attackSlider.value = sliderValue;
                
                if (!(unit.attackDeltaTime <= 0)) continue;
                
                Attack(unit.entity);
                unit.attackDeltaTime = unit.entity.attackTime;
            }
            else
            {
                unit.attackSlider.value = 0;
            }
        }
    }

    private void UpdateText(float currency, TMP_Text text, string message)
    {
        text.text = message + currency;
    }

    private void Attack(Entity attacker)
    {
        switch (attacker.entityType)
        {
            case EntityType.Player:
                DealDamage(EntityType.Enemy, attacker);
                break;
            case EntityType.Enemy:
                DealDamage(EntityType.Player, attacker);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Click()
    {
        DealClickerDamage(clickerComponent.targetUnit.entityType, clickerComponent.attackerUnits[0], clickerComponent.baseClickerDamage);
        DealClickerDamage(clickerComponent.targetUnit.entityType, clickerComponent.attackerUnits[1], clickerComponent.baseClickerDamage);
    }

    private void DealClickerDamage(EntityType entityType, Entity entity, float clickerBaseDamage)
    {
        if (entity.currentStamina.Get() <= 0)
        {
            return;
        }
        entity.currentStamina.Set(entity.currentStamina.Get() - 1);
        
        List<Unit> attackedUnits = units.FindAll(x => x.entity.entityType == entityType);

        if (attackedUnits.Exists(x => x.entity.currentHealth.Get() - clickerBaseDamage > 0))
        {
            foreach (var attackedUnit in attackedUnits)
            {
                attackedUnit.entity.currentHealth.Set(attackedUnit.entity.currentHealth.Get() - clickerBaseDamage);
                attackedUnit.healthSlider.value = attackedUnit.entity.currentHealth.Get() / attackedUnit.entity.totalHealth.Get();
            }
        }
        else
        {
            if (entityType == EntityType.Enemy)
            {
                NextStage();
            }
            else
            {
                RestartStage();
            }
        }
    }

    private void DealDamage(EntityType entityType, Entity entity)
    {
        List<Unit> attackedUnits = units.FindAll(x => x.entity.entityType == entityType);

        if (attackedUnits.Exists(x => x.entity.currentHealth.Get() - (entity.atk.Get() - x.entity.def.Get()) > 0))
        {
            foreach (var attackedUnit in attackedUnits)
            {
                float damage = Mathf.Max(0, entity.atk.Get() - attackedUnit.entity.def.Get());
                attackedUnit.entity.currentHealth.Set(attackedUnit.entity.currentHealth.Get() - damage);
                attackedUnit.healthSlider.value = attackedUnit.entity.currentHealth.Get() / attackedUnit.entity.totalHealth.Get();
            }
        }
        else
        {
            if (entityType == EntityType.Enemy)
            {
                NextStage();
            }
            else
            {
                RestartStage();
            }
        }
    }

    private void NextStage()
    {
        stage++;
        stageText.text = "Stage: " + stage;

        foreach (var scalingStat in scalingStats.stats)
        {
            scalingStat.stat.Set(scalingStat.stat.Get() + scalingStat.valueIncrease);
        }
        
        moneyReward.AddMoney(stage);
        
        RestartStage();
    }

    private void RestartStage()
    {
        foreach (var unit in units)
        {
            unit.entity.currentHealth.Set(unit.entity.totalHealth.Get());
            unit.healthSlider.value = unit.entity.currentHealth.Get() / unit.entity.totalHealth.Get();
            unit.attackDeltaTime = unit.entity.attackTime;
        }
    }

    private void UpdateHealth(float newHealth)
    {
        foreach (var unit in units)
        {
            unit.healthSlider.value = unit.entity.currentHealth.Get() / unit.entity.totalHealth.Get();
        }
    }
}

[Serializable]
public class Unit
{
    public Entity entity;
    public float attackDeltaTime;
    public Slider attackSlider;
    public Slider healthSlider;
    public Slider staminaSlider;
    public float staminaDeltaTime;
}
