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
	public abstract bool CheckRecipe(AtomType[] Ingredients);
}
public class UnshapedRecipe : Recipe
{
	protected Dictionary<AtomType,int> Input;
	public Dictionary<AtomType,int> GetInput() {
		return Input;
	}
	public override bool CheckRecipe(AtomType[] Ingredients){ //partital match unsupported
		Dictionary<AtomType,int> Dim = new(Input);
		foreach (AtomType atoms in Ingredients){
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
	public override bool CheckRecipe(AtomType[] Ingredients){ //partital match unsupported
		MultipleMatcher[] Dim = Input;
		if (Dim.Length != Ingredients.Length) {
			return false;
		}
		for (int index = 0;index < Dim.Length;index++){
			if (Ingredients[index] is not null && Dim[index] is not null) {
				if (Dim[index] != Ingredients[index])
				{
					return false;
				}
			} else if (!(Ingredients[index] is null && Dim[index] is null)){
				return false;
			}
		}
		return true;
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