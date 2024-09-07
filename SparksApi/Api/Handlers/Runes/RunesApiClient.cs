using SparksApi.Api.Models;
using System.Collections.Frozen;

namespace SparksApi.Api.Handlers.Runes;

public sealed class RunesApiClient : IRunesApiClient
{
    private readonly FrozenDictionary<int, Rune> _runeCache;

    private readonly string _baseIconUrl =
        "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/styles";

    public RunesApiClient()
    {
        // hardcoded since the actual API from riot, data dragon or community dragon is not what I needed
        _runeCache = new Dictionary<int, Rune>()
        {
            // ******************** PRESICION TREE ********************
            {8000, new (8000, "Precision", "Improved attacks and sustained damage", "Precision",
                $"{_baseIconUrl}/7201_precision.png")},

            {8005, new (8005, "Press the Attack", "Hitting an enemy champion with 3 consecutive basic attacks deals bonus adaptive damage", "Precision",
                $"{_baseIconUrl}/precision/presstheattack/presstheattack.png")},

            {8021, new (8021, "Fleet Footwork", "Attacking and moving builds Energy stacks. At 100 stacks, your next attack heals you and grants increased movement speed", "Precision",
                $"{_baseIconUrl}/precision/fleetfootwork/fleetfootwork.png")},

            {8010, new (8010, "Conqueror", "Gain stacks of adaptive force when attacking enemy champions. After reaching 12 stacks, heal for a portion of damage you deal to champions.", "Precision",
                $"{_baseIconUrl}/precision/conqueror/conqueror.png")},

            {9101, new (9101, "Absorb Life", "Killing a target heals you.", "Precision",
                $"{_baseIconUrl}/precision/absorblife/absorblife.png")},

            {9111, new (9111, "Triumph", "Takedowns restore health based on missing HP and grant an additional gold", "Precision",
                    $"{_baseIconUrl}/precision/triumph.png")},

            {8009, new (8009, "Presence of Mind", "Increase your mana or energy regeneration when damaging an enemy champion. Takedowns restore mana or energy.", "Precision",
                $"{_baseIconUrl}/precision/presenceofmind/presenceofmind.png")},

            {9104, new (9104, "Legend: Alacrity", "Gain 3% attack speed plus an additional 1.5% for every Legend stack (max. 10 stacks)", "Precision",
                $"{_baseIconUrl}/precision/legendalacrity/legendalacrity.png")},

            {9105, new (9105, "Legend: Haste", "Gain 1.5 basic ability haste for every stack (max. 10 stacks)", "Precision",
                $"{_baseIconUrl}/precision/legendhaste/legendhaste.png")},

            {9103, new (9103, "Legend: Bloodline", "Takedowns on enemies grant permanent Life Steal, up to a cap. Once the cap is reached, increase your max health.", "Precision",
                $"{_baseIconUrl}/precision/legendbloodline/legendbloodline.png")},

            {8014, new (8014, "Coup de Grace", "Deal 8% more damage to champions who have less than 40% health", "Precision",
                $"{_baseIconUrl}/precision/coupdegrace/coupdegrace.png")},

            {8017, new (8017, "Cut Down", "Deal 8% more damage to champions who have more than 50% health", "Precision",
                $"{_baseIconUrl}/precision/cutdown/cutdown.png")},

            {8299, new (8299, "Last Stand", "Deal 5% - 11% increased damage to champions while you are below 60% health. Max damage gained at 30% health.", "Precision",
                $"{_baseIconUrl}/sorcery/laststand/laststand.png")}, // they unfortunately have last stand under sorcery instead of precision
            
            // ******************** DOMINATION TREE ********************
            {8100, new (8100, "Domination", "Burst damage and target access", "Domination",
                $"{_baseIconUrl}/7200_domination.png")},

            {8112, new (8112, "Electrocute", "Hitting a champion with 3 separate attacks or abilities within 3s deals bonus adaptive damage", "Domination",
                $"{_baseIconUrl}/domination/electrocute/electrocute.png")},

            {8128, new (8128, "Dark Harvest", "Damaging a champion below 50% health deals adaptive damage and harvests their soul, permanently increasing Dark Harvest's damage by 5", "Domination",
                $"{_baseIconUrl}/domination/darkharvest/darkharvest.png")},

            {9923, new (9923, "Hail of Blades", "Gain attack speed when you attack an enemy champion for up to 3 attacks", "Domination",
                $"{_baseIconUrl}/domination/hailofblades/hailofblades.png")},

            {8126, new (8126, "Cheap Shot", "Deal bonus true damage to enemy champions with impaired movement or actions.", "Domination",
                $"{_baseIconUrl}/domination/cheapshot/cheapshot.png")},

            {8139, new (8139, "Taste of Blood", "Heal when you damage an enemy champion.", "Domination",
                $"{_baseIconUrl}/domination/tasteofblood/greenterror_tasteofblood.png")},

            {8143, new (8143, "Sudden Impact", "Damaging basic attacks and abilities deal bonus true damage to enemy champions after using a dash, leap, blink, teleport, or when leaving stealth", "Domination",
                $"{_baseIconUrl}/domination/suddenimpact/suddenimpact.png")},

            {8136, new (8136, "Zombie Ward", "After killing an enemy ward, a friendly Zombie Ward is raised in its place. Gain permanent AD or AP for each Zombie Ward spawned plus bonus upon completion.", "Domination",
                $"{_baseIconUrl}/domination/zombieward/zombieward.png")},

            {8120, new (8120, "Ghost Poro",
                "When your wards expire, they leave behind a Ghost Poro. The Ghost Poro grants vision until discovered. Gain permanent AD or AP, adaptive for each Ghost Poro and when your Ghost Poro spots an enemy champion, plus bonus upon completion.",
                "Domination",
                $"{_baseIconUrl}/domination/ghostporo/ghostporo.png")},

            {8138, new (8138, "Eyeball Collection", "Collect eyeballs for champion takedowns. Gain permanent AD or AP, adaptive for each eyeball plus bonus upon collection completion.", "Domination",
                $"{_baseIconUrl}/domination/eyeballcollection/eyeballcollection.png")},

            {8135, new (8135, "Treasure Hunter", "Unique takedowns grant additional gold the first time they are collected.", "Domination",
                $"{_baseIconUrl}/domination/treasurehunter/treasurehunter.png")},

            {8105, new (8105, "Relentless Hunter", "Unique takedowns grant permanent out of combat movement speed.", "Domination",
                $"{_baseIconUrl}/domination/relentlesshunter/relentlesshunter.png")},

            {8106, new (8106, "Ultimate Hunter", "Unique takedowns grant permanent cooldown reduction on your Ultimate.", "Domination",
                $"{_baseIconUrl}/domination/ultimatehunter/ultimatehunter.png")},
            
            // ******************** SORCERY TREE ********************
            {8200, new (8200, "Sorcery", "Empowered abilities and resource manipulation", "Sorcery",
                $"{_baseIconUrl}/7202_sorcery.png")},

            {8214, new (8214, "Summon Aery", "Your attacks and abilities send Aery to a target, damaging enemies or shielding allies", "Sorcery",
                $"{_baseIconUrl}/sorcery/summonaery/summonaery.png")},

            {8229, new (8229, "Comet", "Damaging a champion with an ability hurls a comet at their location", "Sorcery",
                $"{_baseIconUrl}/sorcery/arcanecomet/arcanecomet.png")},

            {8230, new (8230, "Phase Rush", "Hitting an enemy champion with 3 attacks or separate abilities grants a burst of MS", "Sorcery",
                $"{_baseIconUrl}/sorcery/phaserush/phaserush.png")},

            {8224, new (8224, "Nullifying Orb", "Gain a magic damage shield when taken to low health by magic damage.", "Sorcery",
                $"{_baseIconUrl}/sorcery/nullifyingorb/nullifyingorb.png")},

            {8226, new (8226, "Manaflow Band", "Hitting an enemy champion with an ability permanently increases your maximum mana. After reaching 250 bonus mana, restore 1% of your missing mana every 5 seconds.", "Sorcery",
                $"{_baseIconUrl}/sorcery/manaflowband/manaflowband.png")},

            {8275, new (8275, "Nimbus Cloak", "After casting a Summoner Spell, gain a short Move Speed increase that allows you to pass through units.", "Sorcery",
                $"{_baseIconUrl}/sorcery/nimbuscloak/nimbuscloak.png")},

            {8210, new (8210, "Transcendence", "Gain bonuses upon reaching the following levels: Level 5: +5 Ability Haste Level 8: +5 Ability Haste Level 11: On Champion takedown, reduce the remaining cooldown of basic abilities by 20%.", "Sorcery",
                $"{_baseIconUrl}/sorcery/transcendence/transcendence.png")},

            {8234, new (8234, "Celerity", "All movement Speed bonuses are 7% more effective on you and gain 1% movement speed.", "Sorcery",
                $"{_baseIconUrl}/sorcery/celerity/celeritytemp.png")},

            {8233, new (8233, "Absolute Focus", "While above 70% health, gain extra adaptive damage.", "Sorcery",
                $"{_baseIconUrl}/sorcery/absolutefocus/absolutefocus.png")},

            {8237, new (8237, "Scorch", "Your first damaging ability hit every 10s burns champions.", "Sorcery",
                $"{_baseIconUrl}/sorcery/scorch/scorch.png")},

            {8232, new (8232, "Waterwalking", "Gain MS and AP or AD, adaptive in the river.", "Sorcery",
                $"{_baseIconUrl}/sorcery/waterwalking/waterwalking.png")},

            {8236, new (8236, "Gathering Storm", "Gain increasing amounts of AD or AP, adaptive over the course of the game", "Sorcery",
                $"{_baseIconUrl}/sorcery/gatheringstorm/gatheringstorm.png")},
            
            // ******************** INSPIRATION TREE ********************
            {8300, new (8300, "Inspiration", "Creative tools and rule bending", "Inspiration",
                $"{_baseIconUrl}/7203_whimsy.png")},

            {8360, new (8360, "Unsealed Spellbook", "Swap one of your equipped Summoner Spells to a new, single use Summoner Spell", "Inspiration",
                $"{_baseIconUrl}/inspiration/unsealedspellbook/unsealedspellbook.png")},

            {8351, new (8351, "Glacial Augment", "Basic attacking a champion slows them for 2s", "Inspiration",
                $"{_baseIconUrl}/inspiration/glacialaugment/glacialaugment.png")},

            {8369, new (8369, "First Strike", "When you initiate champion combat, deal extra damage for 3 seconds and gain gold based on damage dealt.", "Inspiration",
                $"{_baseIconUrl}/inspiration/firststrike/firststrike.png")},

            {8306, new (8306, "Hextech Flashtraption", "While Flash is on cooldown it is replaced by Hexflash. Hexflash: Channel, then blink to a new location.", "Inspiration",
                $"{_baseIconUrl}/inspiration/hextechflashtraption/hextechflashtraption.png")},

            {8304, new (8304, "Magical Footwear", "You get free boots at 12 min but you cannot buy boots before then. Each takedown you get makes your boots come 45s sooner.", "Inspiration",
                $"{_baseIconUrl}/inspiration/magicalfootwear/magicalfootwear.png")},

            {8321, new (8321, "Cash Back", "Get some gold back when you purchase Legendary Items.", "Inspiration",
                $"{_baseIconUrl}/inspiration/cashback/cashback.png")},

            {8313, new (8313, "Triple Tonic", "Upon reaching level 3, gain an Elixir of Avarice. Upon reaching level 6, gain an Elixir of Force.<br>Upon reaching level 9, gain an Elixir of Skill.", "Inspiration",
                $"{_baseIconUrl}/inspiration/tripletonic/tripletonic.png")},

            {8352, new (8352, "Time Warp Tonic", "Potions grant some restoration immediately.", "Inspiration",
                $"{_baseIconUrl}/inspiration/timewarptonic/timewarptonic.png")},

            {8345, new (8345, "BiscuitDelivery", "Gain a free Biscuit every 2 min, until 6 min. Consuming or selling a Biscuit permanently increases your max mana and restores health and mana.", "Inspiration",
                $"{_baseIconUrl}/inspiration/biscuitdelivery/biscuitdelivery.png")},

            {8347, new (8347, "Cosmic Insight", "+18 Summoner Spell Hast +10 Item Haste", "Inspiration",
                $"{_baseIconUrl}/inspiration/cosmicinsight/cosmicinsight.png")},

            {8410, new (8410, "Approach Velocity", "Bonus movement speed towards nearby enemy champions that are movement impaired, increased for enemy champions that you impair.", "Inspiration",
                $"{_baseIconUrl}/inspiration/approachvelocity/approachvelocity.png")},

            {8316, new (8316, "Jack Of All Trades", "For each different stat gained from items, gain one Jack stack. Each stack grants you 1 Ability Haste. Gain bonus Adaptive Force at 5 and 10 stacks.", "Inspiration",
                $"{_baseIconUrl}/inspiration/jackofalltrades/jackofalltrades.png")},
            
            // ******************** RESOLVE TREE ********************
            {8400, new (8400, "Resolve", "Durability and crowd control", "Resolve",
                $"{_baseIconUrl}/7204_resolve.png")},

            {8439, new (8439, "Aftershock", "After immobilizing an enemy champion, increase your Armor and Magic Resist by 70 + 50% for 2.5s", "Resolve",
                $"{_baseIconUrl}/resolve/veteranaftershock/veteranaftershock.png")},

            {8437, new (8437, "Grasp of the Undying", "Every 4s in combat, your next attack on a champion will deal bonus damage and heal you", "Resolve",
                $"{_baseIconUrl}/resolve/graspoftheundying/graspoftheundying.png")},

            {8465, new (8465, "Guardian", "Guard allies you cast spells on and those that are very nearby", "Resolve",
                $"{_baseIconUrl}/resolve/guardian/guardian.png")},

            {8446, new (8446, "Demolish", "Charge up a powerful attack against a tower while near it.", "Resolve",
                $"{_baseIconUrl}/resolve/demolish/demolish.png")},

            {8463, new (8463, "Font of Life", "Impairing the movement of an enemy champion heals nearby allied champions.", "Resolve",
                $"{_baseIconUrl}/resolve/fontoflife/fontoflife.png")},

            {8401, new (8401, "Shield Bash", "Whenever you gain a shield, your next basic attack against a champion deals bonus adaptive damage.", "Resolve",
                "https://static.wikia.nocookie.net/leagueoflegends/images/9/93/Shield_Bash_rune.png")},

            {8429, new (8429, "Conditioning", "After 12 min gain Armor and Magic Resist and increase your bonus Armor and Magic Resist by 3%.", "Resolve",
                $"{_baseIconUrl}/resolve/conditioning/conditioning.png")},

            {8444, new (8444, "Second Wind", "After taking damage from an enemy champion heal back some missing health over time.", "Resolve",
                $"{_baseIconUrl}/resolve/secondwind/secondwind.png")},

            {8451, new (8451, "Overgrowth", "After taking damage from an enemy champion, the next 3 spells or attacks you receive from them deal less damage.", "Resolve",
                $"{_baseIconUrl}/resolve/overgrowth/overgrowth.png")},

            {8453, new (8453, "Revitalize", "Gain 5% Heal and Shield Power. Heals and shields you cast or receive are 10% stronger on targets below 40% health.", "Resolve",
                $"{_baseIconUrl}/resolve/revitalize/revitalize.png")},

            {8242, new (8242, "Unflinching", "Gain Armor and Magic Resist when receiving crowd control.", "Resolve",
                $"{_baseIconUrl}/sorcery/unflinching/unflinching.png")},
        }.ToFrozenDictionary();
    }

    public IEnumerable<Rune> GetRunesFromPerks(Perks perks) =>
        perks.Styles
            .SelectMany(style => style.Selections)
            .Select(selection => GetRune(selection.Perk))
            .Where(rune => rune != null)
            .Select(rune => rune!);

    private Rune? GetRune(int id) => _runeCache.GetValueOrDefault(id);
}