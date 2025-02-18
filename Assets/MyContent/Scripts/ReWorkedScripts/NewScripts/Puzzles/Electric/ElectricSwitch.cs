﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectricSwitch : MonoBehaviour, IElectricObject
{
    public bool isElectrified { get => _isElectrified; set => _isElectrified = value; }
    private bool _isElectrified;


    public float energyRequiredToActivate;
    public float energyCharge;
    public UnityEvent onSwitchActivated;
    public UnityEvent onElectrified;
    public UnityEvent onSwitchDeactivated;

    private float _actualCharge;
    private bool _fullCharged;

    private void Start()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    public void Electrify()
    {
        _isElectrified = true;
    }

    private void Execute()
    {
        if (_isElectrified)
        {
            _isElectrified = false;
            if(_actualCharge< energyRequiredToActivate)
            {
                onElectrified.Invoke();
                _actualCharge += Time.deltaTime * energyCharge;
                Debug.Log("Charging");
            }
            else
            {
                Debug.Log("Charged");
                _fullCharged = true;
                _isElectrified = false;
                UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
                onSwitchActivated.Invoke();
            }
        }
        else
        {
            if (_actualCharge > 0)
            {
                _actualCharge -= Time.deltaTime * energyCharge;
            }
        }
    }

    public void Restart()
    {
        if (_fullCharged)
        {
            Start();
        }
        Debug.Log("Restart: " + gameObject.name + "Full charge: " + _fullCharged);
        _actualCharge = 0;
        _fullCharged = false;
        _isElectrified = false;
        onSwitchDeactivated.Invoke();
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);

    }
}
