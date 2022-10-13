using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace SynOblivionInteractionIcons
{
    public class Program
    {
        private static readonly ModKey KeyOblivIcon = ModKey.FromNameAndExtension("skymojibase.esl");
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "OblivionInteractionIcons.esp").AddRunnabilityCheck(state =>
                {
                    state.LoadOrder.AssertHasMod(KeyOblivIcon, true, "\n\nskymojibase.esl missing!\n\n");
                })
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            var OblivionIconInteractorESP = state.LoadOrder.GetIfEnabled(KeyOblivIcon);

            if (OblivionIconInteractorESP.Mod == null) return;

            List<FormKey>? OIIFlora = OblivionIconInteractorESP.Mod.Florae.Select(x => x.FormKey).ToList();
            List<FormKey>? OIIActivators = OblivionIconInteractorESP.Mod.Activators.Select(x => x.FormKey).ToList();

            List<IFloraGetter>? winningFlora = state.LoadOrder.PriorityOrder.WinningOverrides<IFloraGetter>().Where(x => OIIFlora.Contains(x.FormKey)).ToList();
            List<IActivatorGetter>? winningActivator = state.LoadOrder.PriorityOrder.WinningOverrides<IActivatorGetter>().Where(x => OIIActivators.Contains(x.FormKey)).ToList();


            Console.WriteLine("Patching Flora");
            foreach (var flora in state.LoadOrder.PriorityOrder.OnlyEnabled().Flora().WinningOverrides())
            {
                // Mushrooms
                if (flora.HarvestSound.FormKey.Equals(Skyrim.SoundDescriptor.ITMIngredientMushroomUp.FormKey))
                {

                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "<font face=\"Iconographia\">A</font>";
                }
                else if (flora.Name.String != null
                         && (flora.Name.String.ToUpper().Contains("SPORE") || flora.Name.String.ToUpper().Contains("CAP") || flora.Name.String.ToUpper().Contains("CROWN") || flora.Name.String.ToUpper().Contains("SHROOM")))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "<font face=\"Iconographia\">A</font>";
                }
                // Clams
                else if (flora.HarvestSound.FormKey.Equals(Skyrim.SoundDescriptor.ITMIngredientClamUp.FormKey) || (flora.Name.String != null && flora.Name.String.ToUpper().Contains("CLAM")))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "<font face=\"Iconographia\">b</Font>";
                }
                // Fill
                else if (flora.HarvestSound.FormKey.Equals(Skyrim.SoundDescriptor.ITMPotionUpSD.FormKey) || (flora.ActivateTextOverride != null && flora.ActivateTextOverride.String != null && flora.ActivateTextOverride.String.ToUpper().Contains("FILL BOTTLES")))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "<font face=\"Iconographia\">L</font>";
                }
                // Cask or Barrel
                else if (flora.Name.String != null
                    && (flora.Name.String.ToUpper().Contains("BARREL") || flora.Name.String.ToUpper().Contains("CASK")))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "<font face=\"Iconographia\">L</font>";
                }
                // Coin Pouch
                else if (flora.HarvestSound.FormKey.Equals(Skyrim.SoundDescriptor.ITMCoinPouchUp.FormKey) || flora.HarvestSound.FormKey.Equals(Skyrim.SoundDescriptor.ITMCoinPouchDown.FormKey) || (flora.Name.String != null && flora.Name.String.ToUpper().Contains("COIN PURSE")))
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "<font face=\"Iconographia\">S</font>";
                }
                // Other Flora
                else
                {
                    var floraPatch = state.PatchMod.Florae.GetOrAddAsOverride(flora);
                    floraPatch.ActivateTextOverride = "<font face=\"Iconographia\">Q</font>";
                }
            }

            foreach  (var flora in OblivionIconInteractorESP.Mod.Florae)
            {
                var winningOverride = winningFlora.Where(x => x.FormKey == flora.FormKey).First();
                var PatchFlora = state.PatchMod.Florae.GetOrAddAsOverride(winningOverride);

                if (flora.ActivateTextOverride == null) continue;
                PatchFlora.ActivateTextOverride = flora.ActivateTextOverride.String;
            }
            

            Console.WriteLine("Patching Activators");
            foreach (var activator in state.LoadOrder.PriorityOrder.OnlyEnabled().Activator().WinningOverrides())
            {
                //Blacklisting superfluos entries
                if (activator.EditorID != null && activator.EditorID.ToString() != null && activator.ActivateTextOverride == null && (activator.EditorID.ToString().ToUpper().Contains("TRIGGER") || activator.EditorID.ToString().ToUpper().Contains("FX")))
                {
                    continue;
                }
                // Search
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("SEARCH")) 
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">V</font>"; 
                }
                // Grab & Touch
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && (activator.ActivateTextOverride.String.ToUpper().Equals("GRAB") || activator.ActivateTextOverride.String.ToUpper().Equals("TOUCH"))) 
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">S</font>"; 
                }
                // Levers
                else if (activator.Keywords != null && activator.Keywords.Contains(Skyrim.Keyword.ActivatorLever.FormKey) || (activator.Name != null && activator.Name.String != null && activator.Name.String.ToUpper().Contains("LEVER"))) 
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">D</font>"; 
                }
                else if (activator.EditorID != null && activator.EditorID.ToString() != null && activator.EditorID.ToString().ToUpper().Contains("PULLBAR"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">D</font>";
                }
                // Chains
                else if (activator.Name != null && activator.Name.String != null && activator.Name.String.ToUpper().Contains("CHAIN"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">E</font>";
                }
                // Mine
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("MINE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">G</font>";
                }
                // Button, Examine , Push, Investigate
                else if (activator.Name != null && activator.Name.String != null && activator.Name.String.ToUpper().Contains("BUTTON") || activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && (activator.ActivateTextOverride.String.ToUpper().Equals("EXAMINE") || activator.ActivateTextOverride.String.ToUpper().Equals("PUSH") || activator.ActivateTextOverride.String.ToUpper().Equals("INVESTIGATE")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">F</font>";
                }
                // Write
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("WRITE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">H</font>";
                }
                // Pray
                else if (activator.Name != null && activator.Name.String != null && (activator.Name.String.ToUpper().Contains("SHRINE") || activator.Name.String.ToUpper().Contains("ALTAR")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">C</font>";
                }
                else if (activator.EditorID != null && activator.EditorID.ToString() != null && activator.EditorID.ToString().ToUpper().Contains("DLC2STANDINGSTONE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">C</font>";
                }
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && (activator.ActivateTextOverride.String.ToUpper().Equals("PRAY") || activator.ActivateTextOverride.String.ToUpper().Equals("WORSHIP")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">C</font>";
                }
                // Drink
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("DRINK"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">J</font>";
                }
                // Eat
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("DRINK"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">K</font>";
                }
                // Drop or Place
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && (activator.ActivateTextOverride.String.ToUpper().Equals("DROP") || activator.ActivateTextOverride.String.ToUpper().Equals("PLACE")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">N</font>";
                }
                // Pick up
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("PICK UP"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">O</font>";
                }
                // Read
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("READ"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">P</font>";
                }
                // Harvest
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("HARVEST"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">Q</font>";
                }
                // Take
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("TAKE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">S</font>";
                }
                // Talk
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("TALK"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">T</font>";
                }
                // Sit
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("SIT"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">U</font>";
                }
                // Open
                else if (activator.Name!= null && activator.Name.String != null && activator.Name.String.ToUpper().Contains("CHEST") && activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("OPEN"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">V</font>";
                }
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("OPEN"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">X</font>";
                }
                // Activate
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("ACTIVATE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">Y</font>";
                }
                // Unlock
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("UNLOCK"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">Z</font>";
                }
                // Sleep
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("SLEEP"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">a</font>";
                }
                // Steal
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("STEAL"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font color='ff0000'><font face=\"Iconographia\">S</font></font>";
                }
                // Pickpocket
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("PICKPOCKET"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font color='ff0000'><font face=\"Iconographia\">b</Font></font>";
                }
                // CLOSE
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("CLOSE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font color='dddddd'><font face=\"Iconographia\">X</font></font>";
                }
                // Steal From
                else if (activator.ActivateTextOverride != null && activator.ActivateTextOverride.String != null && activator.ActivateTextOverride.String.ToUpper().Equals("STEAL FROM"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font color='ff0000'><font face=\"Iconographia\">V</font></font>";
                }
                // Sleep
                else if (activator.Name != null && activator.Name.String != null && (activator.Name.String.ToUpper().Contains("BED") || activator.Name.String.ToUpper().Contains("HAMMOCK") || activator.Name.String.ToUpper().Contains("COFFIN")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">a</font>";
                }
                // Civil War Map
                else if (activator.EditorID != null && activator.EditorID.ToString() != null && (activator.EditorID.ToString().ToUpper().Contains("CWMap")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">F</font>";
                }
				// EVG Ladder
                else if (activator.EditorID != null && activator.EditorID.ToString() != null && activator.EditorID.ToString().ToUpper().Contains("LADDER"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">d</font>";
                }
				 // EVG Squeeze
                else if (activator.EditorID != null && activator.EditorID.ToString() != null && activator.EditorID.ToString().ToUpper().Contains("SQUEEZE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">e</font>";
                }
				// CC Fishing
                else if (activator.Name != null && activator.Name.String != null && (activator.Name.String.ToUpper().Contains("FISHING SUPPLIES")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">I</font>";
                }
				// Torch
                else if (activator.EditorID != null && activator.EditorID.ToString() != null && activator.EditorID.ToString().ToUpper().Contains("TORCHSCONCE"))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">i</font>";
                }
				// dragonclaw
                else if (activator.Name != null && activator.Name.String != null && (activator.Name.String.ToUpper().Contains("KEYHOLE")))
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">j</font>";
                }
                else
                {
                    var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(activator);
                    activatorPatch.ActivateTextOverride = "<font face=\"Iconographia\">W</font>";
                }
                
            }

            foreach (var activator in OblivionIconInteractorESP.Mod.Activators)
            {
                var winningOverride = winningActivator.Where(x => x.FormKey == activator.FormKey).First();
                var activatorPatch = state.PatchMod.Activators.GetOrAddAsOverride(winningOverride);

                if (activator.ActivateTextOverride == null) continue;
                activatorPatch.ActivateTextOverride = activator.ActivateTextOverride.String;
            }
        }
    }
}
