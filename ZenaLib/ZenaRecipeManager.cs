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

public static class RecipeManager
{
    public static bool CheckUnshapedRecipe(AtomType[] Input, UnshapedRecipe Recipe, out AtomType[] Output){ //partital match unsupported
        Dictionary<AtomType,int> Dim = new(Recipe.GetInput());
        Output = Recipe.GetOutput();
        foreach (AtomType atoms in Input){
            if (atoms is null) {
                continue;
            } else if (Dim.ContainsKey(atoms)) {
                Dim[atoms]--;
                if (Dim[atoms] <= 0) {
                    Dim.Remove(atoms);
                }
            } else {
                return false;
            }
        }
        return Dim.Count <= 0;
    }
    public static bool CheckShapedRecipe(AtomType[] Input, ShapedRecipe Recipe, out AtomType[] Output){ //partital match unsupported
        MultipleMatcher[] Dim = Recipe.GetInput();
        Output = Recipe.GetOutput();
        if (Dim.Length != Output.Length) {
            return false;
        }
        for (int index = 0;index <= Dim.Length;index++){
            if (Input[index] is not null && Dim[index] is not null) {
                if (Dim[index] != Input[index])
                {
                    return false;
                }
            } else if (!(Input[index] is null && Dim[index] is null)){
                return false;
            }
        }
        return true;
    }
    public static bool CheckRecipes(AtomType[] Input, Recipe[] Recipe, out AtomType[] Output){ //partital match unsupported
        foreach (Recipe EachRecipe in Recipe){
            if (EachRecipe is ShapedRecipe Shape){
                if (CheckShapedRecipe(Input,Shape,out AtomType[] Outo)){
                    Output = Outo;
                    return true;
                }
            }
            if (EachRecipe is UnshapedRecipe Freeform){
                if (CheckUnshapedRecipe(Input,Freeform,out AtomType[] Outo)){
                    Output = Outo;
                    return true;
                }
            }
        }
        Output = new AtomType[0];
        return false;
    }
}