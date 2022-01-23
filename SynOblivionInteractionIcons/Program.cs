using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;
using Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace SynOblivionInteractionIcons
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "OblivionInteractionIcons.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            Console.WriteLine("Patching Flora");
            foreach (var flora in state.LoadOrder.PriorityOrder.OnlyEnabled().Flora().WinningOverrides())
            {
                // Mushrooms
                if (flora.HarvestSound.Equals(Skyrim.SoundDescriptor.ITMIngredientMushroomUp.FormKey))
                {

                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "¢";
                }
                else if (flora.Name.String != null
                         && (flora.Name.String.ToUpper().Contains("SPORE") || flora.Name.String.ToUpper().Contains("CAP") || flora.Name.String.ToUpper().Contains("CROWN") || flora.Name.String.ToUpper().Contains("SHROOM")))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "¢";
                }
                // Clams
                else if (flora.HarvestSound.Equals(Skyrim.SoundDescriptor.ITMIngredientClamUp.FormKey))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "¤";
                }
                // Fill
                else if (flora.HarvestSound.Equals(Skyrim.SoundDescriptor.ITMPotionUpSD.FormKey))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "µ";
                }
                // Coin Pouch
                else if (flora.HarvestSound.Equals(Skyrim.SoundDescriptor.ITMCoinPouchUp.FormKey))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "¼";
                }
                // Other Flora
                else
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "½";
                }
            }

            Console.WriteLine("Patching Activators");
            foreach (var activator in state.LoadOrder.PriorityOrder.OnlyEnabled().Activator().WinningOverrides())
            {
                // Search
                if(activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("SEARCH")) 
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¹"; 
                }
                // Grab & Touch
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && (activator.ActivateTextOverride.String.ToUpper().Equals("GRAB") || activator.ActivateTextOverride.String.ToUpper().Equals("TOUCH"))) 
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¼"; 
                }
                // Levers
                else if (activator.Keywords != null && activator.Keywords.Contains(Skyrim.Keyword.ActivatorLever.FormKey) || (activator.Name != null && activator.Name.String != null && activator.Name.String.ToUpper().Contains("LEVER"))) 
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¦"; 
                }
                else if (activator.EditorID != null && activator.EditorID.ToString() != null && activator.EditorID.ToString().ToUpper().Contains("PULLBAR"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¦";
                }
                // Chains
                else if (activator.Name != null && activator.Name.String != null && activator.Name.String.ToUpper().Contains("CHAIN"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "©";
                }
                // Mine
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("MINE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "×";
                }
                // Button
                else if (activator.Name != null && activator.Name.String != null && activator.Name.String.ToUpper().Contains("BUTTON"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "§";
                }
                // Push or Examine
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && (activator.ActivateTextOverride.String.ToUpper().Equals("EXAMINE") || activator.ActivateTextOverride.String.ToUpper().Equals("PUSH")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "§";
                }
                // Write
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("WRITE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¬";
                }
                // Pray
                else if (activator.Name != null && activator.Name.String != null && (activator.Name.String.ToUpper().Contains("SHRINE") || activator.Name.String.ToUpper().Contains("ALTAR")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¥";
                }
                else if (activator.EditorID != null && activator.EditorID.ToString() != null && activator.EditorID.ToString().ToUpper().Contains("DLC2STANDINGSTONE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¥";
                }
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && (activator.ActivateTextOverride.String.ToUpper().Equals("PRAY") || activator.ActivateTextOverride.String.ToUpper().Equals("WORSHIP")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¥";
                }
                // Drink
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("DRINK"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "Ó";
                }
                // Eat
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("DRINK"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "Ø";
                }
                // Drop or Place
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && (activator.ActivateTextOverride.String.ToUpper().Equals("DROP") || activator.ActivateTextOverride.String.ToUpper().Equals("PLACE")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "Ý";
                }
                // Pick up
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("PICK UP"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "®";
                }
                // Read
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("READ"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¾";
                }
                // Harvest
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("HARVEST"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "½";
                }
                // Take
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("TAKE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¼";
                }
                // Talk
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("TALK"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "»";
                }
                // Sit
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("SIT"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "º";
                }
                // Open
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("OPEN"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "¶";
                }
                // Activate
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("ACTIVATE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "«";
                }
                // Unlock
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("UNLOCK"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "°";
                }
                // Sleep
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("SLEEP"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "ª";
                }
                // Steal
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("STEAL"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font color='ff0000'>¼</font>";
                }
                // Pickpocket
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("PICKPOCKET"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font color='ff0000'>¤</font>";
                }
                // CLOSE
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("CLOSE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font color='dddddd'>¶</font>";
                }
                // Steal From
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("STEAL FROM"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font color='ff0000'>¹</font>";
                }
                else
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "³";
                }

            }
        }
    }
}
