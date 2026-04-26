using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;
using Quintessential.Settings;
using SDL2;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace ZenaLib;

public abstract class Recipe
{
    protected AtomType[] Output;
    public AtomType[] GetOutput() {
        return Output;
    }
}
public class UnshapedRecipe : Recipe
{
    protected Dictionary<AtomType,int> Input;
    public Dictionary<AtomType,int> GetInput() {
        return Input;
    }
    public UnshapedRecipe(Dictionary<AtomType,int> Targ,AtomType[] Dispel){
        Input = Targ;
        Output = Dispel;
    }
}
public class ShapedRecipe : Recipe
{
    protected MultipleMatcher[] Input;
    public MultipleMatcher[] GetInput() {
        return Input;
    }
    public ShapedRecipe(MultipleMatcher[] Targ, AtomType[] Dispel){
        Input = Targ;
        Output = Dispel;
    }
}
public class MultipleMatcher
{
    protected AtomType[] Matchers;
    public MultipleMatcher(AtomType[] Dispel){
        Matchers = Dispel;
    }
    public MultipleMatcher(AtomType Dispel){
        Matchers = new AtomType[1]{Dispel};
    }
    public static bool operator ==(MultipleMatcher a, AtomType b){
        foreach (AtomType atem in a.Matchers){
            if (atem == b) {
                return true;
            }
        }
        return false;
    }
    public static bool operator !=(MultipleMatcher a, AtomType b){
        return !(a == b);
    }
    public static bool operator ==(MultipleMatcher a, MultipleMatcher b){
        return a.GetHashCode() == b.GetHashCode();
    }
    public static bool operator !=(MultipleMatcher a, MultipleMatcher b){
        return !(a == b);
    }
}