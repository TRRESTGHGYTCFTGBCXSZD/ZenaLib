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
	public static bool CheckRecipes(AtomType[] Input, Recipe[] Recipe, out AtomType[] Output){
		foreach (Recipe EachRecipe in Recipe){
			
			if (EachRecipe.CheckRecipe(Input)){
				Output = EachRecipe.GetOutput();
				return true;
			}
		}
		Output = new AtomType[0];
		return false;
	}
}