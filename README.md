# sparksapi⚡
A (work in progress) League of Legends API for individual champion, item and runes stats/analytics
> ⚠️ This repository is a work in progress. I am also working on a web application using this api which will be published in another repo in the future.

If you have any suggestions for refactoring, features etc. please open an issue. :)

# infos
In order to run this you would need to have a riot API key which you can get [here](https://developer.riotgames.com/) for free.
Store it into an environment variable called `RIOT_API_KEY`

# Endpoints
You will find the DTOs documented below the endpoint docs.

## Account
### GET: `/Account/accountFromRiotId`
Returns an `Account` based on the given Riot ID and region.

**Parameters**:
- string: name
- string: tagline
- string: region

### GET: `/Account/GetAccountFromPuuid`
Returns an `Account` based on the given puuid and region.

**Parameters**:
- string: puuid
- string: region

## Champion Analyzer
### GET: `/ChampionAnalyzer`
Returns a collection of `ChampionAnalytic` based on puuid, region, matchCount and skip.

**Parameters**:
- string:       puuid
- string:       region
- int:          matchCount  (amount of matches to be fetched)
- int:          skip        (amount of matches of latest matches to be skipped, default: 0)

## Item Analyzer
### GET: `/ItemAnalyzer`
Returns a collection of `ItemAnalytic` based on puuid, region, matchCount and skip.

**Parameters**:
- string:       puuid
- string:       region
- int:          matchCount  (amount of matches to be fetched)
- int:          skip        (amount of matches of latest matches to be skipped, default: 0)

## Match
### GET: `/Match/matchIds`
Returns a collection of `string` (matchIds) based on puuid, region, matchCount and skip.

**Parameters**:
- string:       puuid
- string:       region
- int:          matchCount  (amount of matches to be fetched)
- int:          skip        (amount of matches of latest matches to be skipped, default: 0)

Example Response:
```json
[
  "string",
  ...
]
```

### GET: `/Match/matchesFromPuuid`
Returns a collection of `Match` based on puuid, region, matchCount and skip.

**Parameters**:
- string:       puuid
- string:       region
- int:          matchCount  (amount of matches to be fetched, default: 10)
- int:          skip        (amount of matches of latest matches to be skipped, default: 0)

### POST: `/Match/matchesFromIds`
Returns a collection of `Match` based on given matchIds and region:

**Parameters**:
- string:       region

Request Body Example:
```json
[
  "string",
  ...
]
```

### GET: `/Match/matchHistory`
Returns a collection of `MatchParticipation` based on puuid, region, matchCount and skip.

**Parameters**:
- string:       puuid
- string:       region
- int:          matchCount  (amount of matches to be fetched, default: 10)
- int:          skip        (amount of matches of latest matches to be skipped, default: 0)

## Rune Analyzer
### GET: `/RuneAnalyzer`
Returns a collection of `RuneTreeAnalytic` based on puuid, region, matchCount and skip.

**Parameters**:
- string:       puuid
- string:       region
- int:          matchCount  (amount of matches to be fetched, default: 10)
- int:          skip        (amount of matches of latest matches to be skipped, default: 0)

## Summoner
### GET: `/Summoner`
Returns a `Summoner` based on puuid and region.

****Parameters****:
- string:       puuid
- string:       region

# DTOs / Data Transfer Objects
## Account
```json
{
  "puuid": "string",
  "gameName": "string",
  "tagLine": "string",
  "region": 0
}
```

## Summoner
```json
{
  "accountId": "string",
  "profileIconUrl": "string",
  "revisionDate": 0,
  "id": "string",
  "puuid": "string",
  "summonerLevel": 0
}
```

## ChampionAnalytic
```json
[
  {
    "winRate": 0,
    "pickRate": 0,
    "totalMatches": 0,
    "championName": "string"
  },
  ...
]
```

## ItemAnalytic
```json
[
  {
    "winRate": 0,
    "pickRate": 0,
    "totalMatches": 0,
    "championName": "string",
    "itemName": "string",
    "itemId": 0
  },
  ...
]
```

## RuneTreeAnalytic
> The RuneTreeAnalytic contains the stats/analytics for every rune tree which also contains stats/analytics for every rune
```json
[
  {
    "winRate": 0,
    "pickRate": 0,
    "totalMatches": 0,
    "treeName": "string",
    "championName": "string",
    "runes": [
      {
        "winRate": 0,
        "pickRate": 0,
        "totalMatches": 0,
        "rune": {
          "id": 0,
          "name": "string",
          "description": "string",
          "treeName": "string",
          "iconUrl": "string"
        }
      }
    ],
    ...
  },
  ...
]
```

## MatchParticipation
```json
[
  {
    "gameMode": "string",
    "championIconUrl": "string",
    "champion": "string",
    "items": [
      {
        "id": 0,
        "name": "string",
        "description": "string",
        "stats": [
          "string"
        ],
        "iconUrl": "string",
        "priceTotal": 0
      }
    ],
    "kills": 0,
    "deaths": 0,
    "assists": 0,
    "killDeathAssistRatio": 0,
    "win": true,
    "totalCs": 0,
    "runes": [
      {
        "id": 0,
        "name": "string",
        "description": "string",
        "treeName": "string",
        "iconUrl": "string"
      }
    ]
  }
]
```

## Match
```json
[
  {
    "matchId": "string",
    "participantPuuids": [
      "string"
    ],
    "participants": [
      {
        "assists": 0,
        "champExperience": 0,
        "champLevel": 0,
        "championId": 0,
        "championName": "string",
        "consumablesPurchased": 0,
        "challenges": {
          "abilityUses": 0,
          "damagePerMinute": 0,
          "legendaryItemUsed": [
            0
          ],
          "effectiveHealingAndShielding": 0,
          "goldPerMinute": 0,
          "kda": 0,
          "killParticipation": 0
        },
        "damageDealtToTurrets": 0,
        "damageSelfMitigated": 0,
        "deaths": 0,
        "doubleKills": 0,
        "firstBloodAssist": true,
        "firstBloodKill": true,
        "firstTowerAssist": true,
        "firstTowerKill": true,
        "gameEndedInEarlySurrender": true,
        "gameEndedInSurrender": true,
        "goldEarned": 0,
        "goldSpent": 0,
        "individualPosition": "string",
        "inhibitorTakedowns": 0,
        "inhibitorsLost": 0,
        "item0": 0,
        "item1": 0,
        "item2": 0,
        "item3": 0,
        "item4": 0,
        "item5": 0,
        "item6": 0,
        "itemsPurchased": 0,
        "killingSprees": 0,
        "kills": 0,
        "lane": "string",
        "largestCriticalStrike": 0,
        "largestKillingSpree": 0,
        "largestMultiKill": 0,
        "longestTimeSpentLiving": 0,
        "magicDamageDealt": 0,
        "magicDamageDealtToChampions": 0,
        "magicDamageTaken": 0,
        "neutralMinionsKilled": 0,
        "objectivesStolen": 0,
        "objectivesStolenAssists": 0,
        "participantId": 0,
        "pentakills": 0,
        "physicalDamageDealt": 0,
        "physicalDamageDealtToChampions": 0,
        "physicalDamageTaken": 0,
        "placement": 0,
        "playerAugment1": 0,
        "playerAugment2": 0,
        "playerAugment3": 0,
        "playerAugment4": 0,
        "playerSubteamId": 0,
        "profileIcon": 0,
        "puuid": "string",
        "quadraKills": 0,
        "riotIdGameName": "string",
        "riotIdName": "string",
        "riotIdTagline": "string",
        "role": "string",
        "spell1Casts": 0,
        "spell2Casts": 0,
        "spell3Casts": 0,
        "spell4Casts": 0,
        "subteamPlacement": 0,
        "summoner1Casts": 0,
        "summoner1Id": 0,
        "summoner2Casts": 0,
        "summoner2Id": 0,
        "summonerId": "string",
        "summonerLevel": 0,
        "summonerName": "string",
        "teamEarlySurrendered": true,
        "teamId": 0,
        "teamPosition": "string",
        "timePlayed": 0,
        "totalAllyJungleMinionsKilled": 0,
        "totalDamageDealt": 0,
        "totalDamageDealtToChampions": 0,
        "totalDamageShieldedOnTeammates": 0,
        "totalDamageTaken": 0,
        "totalEnemyJungleMinionsKilled": 0,
        "totalHeal": 0,
        "totalHealsOnTeammates": 0,
        "totalMinionsKilled": 0,
        "totalTimeSpentDead": 0,
        "totalUnitsHealed": 0,
        "tripleKills": 0,
        "trueDamageDealt": 0,
        "trueDamageDealtToChampions": 0,
        "trueDamageTaken": 0,
        "turretsLost": 0,
        "visionScore": 0,
        "win": true,
        "perks": {
          "statPerks": {
            "defense": 0,
            "flex": 0,
            "offense": 0
          },
          "styles": [
            {
              "description": "string",
              "selections": [
                {
                  "perk": 0,
                  "var1": 0,
                  "var2": 0,
                  "var3": 0
                }
              ],
              "style": 0
            }
          ]
        }
      }
    ],
    "gameMode": "string"
  },
  ...
]
```
