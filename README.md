# PokemonGo.RocketAPI
Pokémon GO API

!!! THIS IS NOT A CHEATING BOT !!!

## Details

This is a copy (and refactor) of (FenoxRev's)[https://github.com/FenoxRev] RocketAPI library.

It is ONLY an API client, and it will never be more. While it can be used for cheating, I'd prefer to think that it will only be used for alternative clients on e.g. Windows Mobile devices.


## TODOs

Currently, the client is nowhere near as functional as FenoxRev's final release. This is due to two things:

 - A major refactor following MVVM patterns
 - Moving the library to PCL base, for other platforms
 
The move to PCL is complete and all support libraries have been ported (namely S2Geometry, which is currently included in this repo, but will be moved to its own repo later on).

However this move won't be supported by the VarintByteConverter library used by FenoxRev. He did mention that there is an implementation of the same converter in Google.Protobuf, however I could not find it yet - mainly due to my inexperience of ProtoBuf in general.

The following TODO points are in code:

 - [ ] Fix Varint to byte[] conversion (VarintByteConverter dependency)
 - [ ] Implement all Client calls (currently only two are implemented)
 - [ ] Refactor Login providers with generic interface
 - [ ] Implement GPSOAuth login provider for easier Google login
 
## Wanted help

While all help is accepted, I would like to point out that current focus is on making the API as functional as possible. For this we need people with the following expertise:

 - [ ] Protobuf experience
 - [ ] Unity reverse experience
 
Main focus is getting the most out of the proto files we have currently. This also means that all new releases of the game have to be checked if the protocol changed, and these should be added to the code base.

Later on the API client will declare the support level with a static string, e.g. ´v0.29.3´, and the release date of the library.


## Unwanted things

These are the things I would prefer not to see as a posted issue or pull request:

 - Any requests regarding the usage of the client. It is pretty straight forward, with well documented code.
 - Any requests regarding the usage of the client for cheating. I do NOT want to give any reasons to Niantic to make us pull the project. 
 
