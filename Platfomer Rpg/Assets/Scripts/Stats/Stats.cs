using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stats
{
    [SerializeField] int baseValue;
    public List<int> modifiers;//add or subtract from base state added when equipping or using item
    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }//return the value of the stats after adding all the modifiers
    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }//set base value of stat
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
        
    }
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
