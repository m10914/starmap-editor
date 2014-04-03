//args. - parameters
//		numOfSystems - 
//		seed - 
//
//generator.AddSystem() - adds starsystem
//generator.LinkSystems() - links starsystems
//generator.AddAsteroidToSystem() - adds asteroid
//generator.AddPlanetToStarSystem() - adds planet
//generator.AddNPCShipToSystem() - adds NPC
//generator.AddBaseToSystem() - adds base
//generator.Add ...


include(CSGen.js);
include(NamesGen.js);

using(generator);
using(relations);
using(spawn);
using(console);


var NumOfSystems = 200;
var MinJumpsBetweenCapitals = 4;
var curNumOfSystems = 1; // number of systems created
var systems = [];

//----------------------------------------------
// Name: Generate objects inside the system
// 
function GenSystemObjects(sys_id, args)
{
	MathExt.RandInt();
	// call on lib function
	args.sys_id = sys_id;
	CSGen.CreateGenericSystem( args );
	
	// now stars, planets, asteroids and basic objects are created
	// it's time we create active objects
	
	// Basic objects of every system are Jumpgate and Base,
	// they are always near
	
	// add jumpgate (1)
	var jg_coord = CSGen.GetRandomCoordWithinWorldBounds( args );
	var jg_id = generator.AddJumpgate(sys_id, jg_coord.x, jg_coord.y, "jumpgate_01");
	
	// add base (1)
	var ang = MathExt.RandDouble() * 3.1415 * 2.0;
	var bas_coord = {
		x: jg_coord.x + 35 * Math.cos(ang),
		y: jg_coord.y + 35 * Math.sin(ang),
	};
	var base_id = generator.AddBase(sys_id, bas_coord.x, bas_coord.y, "outpost_01", "GeneratedName");
	relations.SetBaseFaction(base_id, "tech"); //set default base faction to tech
	
	// TEST!!!
	var rotation = { X: (MathExt.RandDouble()) * 90, Y: (MathExt.RandDouble()) * 90, Z: 0.0 };
	var rotationSpeed = { X: (MathExt.RandDouble() * 0.5 - 1.0) * 0.25 * 90, Y: 0.0, Z: 0.0 };
	generator.AddDecoration(sys_id, "satellite_01",
						{x: bas_coord.x, y: bas_coord.y}, 33,
						rotation,
						rotationSpeed,
						1);
						
	generator.AddSpaceObject(sys_id, jg_coord.x + 10, jg_coord.y, "debris_1");
	generator.AddSpaceObject(sys_id, jg_coord.x + 10, jg_coord.y+5, "debris_2");
	generator.AddSpaceObject(sys_id, jg_coord.x + 10, jg_coord.y+10, "debris_3");
	// TEST !!!
	
	
	
	// now we have a base and a jumpgate, it's time to spawn npcs
	
	// first, we determine safe zone - it's an area around base, in which hostiles are not
	// to be spawned
	var safe_radius = 85.0;
	
	// now, we spawn hostile npc's (mostly pirates at this point)
	var pirates = ["pirateship_fang", "pirateship_boomerang", "pirateship_heretic", "pirateship_crab"];
	var density = 40;
	var intensity = 5;
	
	var bounds = CSGen.GetWorldBounds( args );	
	var num = Math.round( bounds.x / density );
	
	for(var i=-num; i < num; i++)
		for(var j=-num; j < num; j++)
		{
			var new_coord = 
			{
				x: (i + MathExt.RandDouble()) * density,
				y: (j + MathExt.RandDouble()) * density
			};
			if(
				MathExt.RandInt() % intensity == 0 //intensity
				&& MathExt.Vector2Length(new_coord, bas_coord) > safe_radius ) //safe zone
			{
				// here we decide which size of group we want to create
				var groupsize = 1;
				if(MathExt.RandInt() % 10 == 0)
					groupsize = MathExt.RandInt() % 3 + 2;
				
				for(var k=0; k < groupsize; k++)
				{
					//now always a pirate
					var shipByRandom = pirates[ MathExt.RandInt() % pirates.length ];
					
					id = generator.AddNPCShipToSystem(
						"Pirate", "PirateInWaiting", shipByRandom, 
						sys_id,
						new_coord.x + MathExt.RandInt()%6,
						new_coord.y + MathExt.RandInt()%6);
						
					relations.SetShipFaction(id, "pirate");
				}
			}
		}
	
	// add neutral npc's to system
	var MinersNum = MathExt.RandInt() % 3 + 2;
	for(var i=0; i < MinersNum; i++)
	{
		var ang = MathExt.RandDouble() * 3.1415 * 2.0;
		var new_coord = {
			x: bas_coord.x + 25 * Math.cos(ang),
			y: bas_coord.y + 25 * Math.sin(ang)
		};
		
		//spawn miner
		var shipByRandom = pirates[ MathExt.RandInt() % pirates.length ];
		id = generator.AddNPCShipToSystem("Tech Miner", "TechMiner", "techship", sys_id, new_coord.x + MathExt.RandInt()%3, new_coord.y + MathExt.RandInt()%3);
		relations.SetShipFaction(id, "tech");
	}
	
	//--------------------------------------
	// Special for start system
	//
	if(sys_id == 1) 
	{
		var id = 0;	
		id = generator.AddNPCShipToSystem("Darth Williams", "TestNpcShip", "techship", sys_id, bas_coord.x - 25, bas_coord.y);
		relations.SetShipFaction(id, "tech");		

		//add spawn info
		spawn.SetCoordinates(jg_coord.x, jg_coord.y);
	}
}	
	
	
//-----------------------------------------------------------
// GenerateGalaxy
// entry point of this script
//-----------------------------------------------------------	
function GenerateGalaxy(args)
{
	//set seed of random
	//MathExt.RandSeed(args.seed);
	
	// generate systems
	GenerateSystems(args);
	
	// generate links
	GenerateLinks(args);
	
	
	//--------------------------
	// O R I G I N (pick)
	//
	var angle = MathExt.RandRangeDouble(0, Math.PI*2);
	
	var start_coord = { x: 50 + 50 * Math.cos(angle), y : 50 + 50 * Math.sin(angle) };
	var start_system_id = generator.GetClosestSystemToPoint(start_coord.x, start_coord.y);
	spawn.SetSystemID(start_system_id);
	console.Print("start system: " + start_system_id);
	
	
	// generate factions
	GenerateFactions(args, start_coord, start_system_id);
	
	// set tech and danger levels
	GenerateSystemLevels(args, start_coord, start_system_id);
	
	// misc
	// ???
}


//--------------------------------------------------------------------------
// Name:
// Desc: function generates systems
//--------------------------------------------------------------------------
function GenerateSystems( args )
{
	var center = { x:50, y: 50 };
	var tries = 20; //max attempts to create one system
	var minDistanceBetweenStars = 3;
	
	curNumOfSystems = 0;
	
	while(curNumOfSystems < NumOfSystems)
	{
		for(var i=0; i < tries; i++)
		{
			var coord = { x: MathExt.RandRangeDouble(5, 95), y: MathExt.RandRangeDouble(5, 95) };
			var dist = MathExt.Vector2Length(coord, center);	
						
			if(dist > 50) //test if coordinate in galaxy circle
			{
				i--;
			}
			else
			{
				var mindist = generator.GetMinDistanceToSystem(coord.x, coord.y);
				if( mindist > minDistanceBetweenStars )
				{
					generator.AddSystem( coord.x, coord.y,
					SystemNameGenerator.GetName(),
					MathExt.RandInt());
					curNumOfSystems++;
					break;
				}			
			}		
		}
		if(i == tries)
		{
			console.Print("Couldn't find a place for star in "+tries+" tries.");
			break;
		}
	}
}


//--------------------------------------------------------------------------
// Name:
// Desc: function generates links between created systems
//--------------------------------------------------------------------------
function GenerateLinks( args )
{
	var GenerationSteps = 5;
	var AverageJumpsFromSystemToSystem = 4;
	
	var ByDist = {};
	var Systems = generator.GetAllSystems();
	
	for(var i=0; i < GenerationSteps; i++)
	{
		for(var j = 0; j < Systems.length; j++)
		{
			// allocate if none
			var curSysID = Systems[j];
			if(typeof(ByDist[curSysID]) == "undefined")
				ByDist[curSysID] = generator.GetSystemsByDistanceTo(curSysID);
				
			// try to connect with current iteration
			var conWithID = ByDist[curSysID][i+1];
			
			if(generator.GetJumpsBetweenSystems(curSysID, conWithID, AverageJumpsFromSystemToSystem) > AverageJumpsFromSystemToSystem)
			{
				generator.AddSystemsLink(curSysID, conWithID);
			}
		}			
	}
	
	// make system whole
	generator.FixConnectivity();
}


//--------------------------------------------------------------------------
// Name:
// Desc: generate tech and danger levels of systems regardless of factions
// ( factions influence will be taken into account later )
//--------------------------------------------------------------------------
function GenerateSystemLevels( args, start_coord, start_system_id )
{
	//vars
	var sys, coord, dist, fraction, fraction2;
	var tech_level, danger_level;
	var factionInfo, capital_id, capital_info;
	
	// set systems default level according to how far it is from origin
	var systems = generator.GetAllSystems();
	for(var i=0; i < systems.length; i++)
	{
		sys = generator.GetSystemByID(systems[i]);
		coord = { x: sys.coord_x, y: sys.coord_y };
		
		dist = MathExt.Vector2Length(coord, start_coord);
		fraction = dist / 100;
		
		tech_level = 5 + fraction*50 + (MathExt.RandDouble()-0.5)*fraction*50;
		danger_level = 2 + Math.round(fraction*70);
		
		// take faction into account
		if(typeof(sys.faction) != "undefined" && sys.faction.length > 0)
		{
			factionInfo = relations.GetFactionInfoByID(sys.faction);
			capital_id = relations.GetFactionCapital(sys.faction);
			capital_info = generator.GetSystemByID(capital_id);
						
			dist = MathExt.Vector2Length(coord, {x: capital_info.coord_x, y: capital_info.coord_y});
			fraction2 = 1 - dist / factionInfo.capital_area_of_influence;
			if(fraction2 > 1) fraction2 = 1;
			else if(fraction2 <= 0) fraction2 = 0;
			
			tech_level += (factionInfo.capital_tech_level - tech_level)*fraction2;
			danger_level += (factionInfo.capital_danger_level - danger_level)*fraction2;
		}
		
		generator.SetSystemTechLevel(systems[i], Math.round(tech_level));	
		generator.SetSystemDangerLevel(systems[i], Math.round(danger_level));
	}
	

}


//--------------------------------------------------------------------------
// Name:
// Desc: function generates factions balance in galaxy
//--------------------------------------------------------------------------
function GenerateFactions( args, start_coord, start_system_id )
{
	//----------------------------------------------
	// C A P I T A L S   of factions
	//
	
	// grab info about factions (from xmls)
	var factions = {};
	var factions_ids = relations.GetFactions();
	for(var i = 0; i < factions_ids.length; i++)
	{
		var fact = relations.GetFactionInfoByID(factions_ids[i]);
		if(fact.stars_count_value != 0 || fact.stars_count_percent != 0)
			factions[factions_ids[i]] =	fact;
	}
		
	var result;
	var tries = 5;
	for(var i = 0 ; i < tries; i++)
	{
		// pick capitals
		result = GenerateFactions_PickCapitals( factions, start_system_id, start_coord );
		if(!result)
		{
			relations.ClearAllInfoOnSystems();
		}
		else
		{
			// log TODO: remove
			for(var i in factions)
			{
				relations.SetFactionCapital(i, factions[i].capital);
				console.Print("Faction " + i + " now has capital " + factions[i].capital);
			}
			
			// expand factions area
			result = GenerateFactions_CreateFactionsAreas(factions, start_system_id);
			if(!result)
			{
				relations.ClearAllInfoOnSystems();
			}
			else break; // generated successfully
		}	
	}
	if(i == tries)
	{
		console.Print("Error: cannot generate factions areas with specified parameters. Try changing Xmls.");
	}
}


//--------------------------------------------------------------------------
// Name:
// Desc: special function to pick capitals according to xml parameters
//--------------------------------------------------------------------------
function GenerateFactions_PickCapitals( factions, start_system_id, start_coord )
{
	// variables
	var i;
	var capitals = [ start_system_id ];
	var capital_coord = {};	
	var ang;
	var nsys_id;
	var bfound;
	var j;
	var i;
	var tries = 200;
	var avDist;
		
	//starting system is on the edge of galaxy, so we determine angle "into" system
	var start_angle = Math.atan2(50-start_coord.y, 50-start_coord.x);
	var spread_angle;
	
	console.Print("start_coord: " + start_coord.x + ";"+start_coord.y + "   angle:" + start_angle);
		
	for(i in factions)
	{		
		for(tryno = 0; tryno < tries; tryno++)
		{	
			// pick distance
			avDist = MathExt.RandRange(
				factions[i].distance_from_origin_min,
				factions[i].distance_from_origin_max );
			spread_angle = Math.acos(avDist / (50 * 2));
		
			// pick coord according to faction parameters
			ang = MathExt.RandRangeDouble(start_angle - spread_angle, start_angle + spread_angle);
	
			capital_coord = {
				x: start_coord.x + avDist * Math.cos(ang),
				y: start_coord.y + avDist * Math.sin(ang),
				};
			
			// check if coord is within our megasphere
			if(MathExt.Vector2Length({x:50,y:50}, capital_coord) <= 50)
			{
				// pick system, check if it's free and far enough from other capitals	
				nsys_id = generator.GetClosestSystemToPoint(capital_coord.x, capital_coord.y);
				
				bfound = false;
				for(j = 0; j < capitals.length; j++)
				{
					if( nsys_id == capitals[j] || 
					generator.GetJumpsBetweenSystems(capitals[j], nsys_id, MinJumpsBetweenCapitals) < MinJumpsBetweenCapitals )
					{
						bfound = true;
						break;
					}
				}
				
				if(!bfound)
				{
					capitals.push(nsys_id);
					factions[i].capital = nsys_id;
					break;
				}
				else
				{
					tryno++;
				}
			}
			else
			{
				console.Print(avDist + " " + ang + " " + capital_coord.x + ";"+capital_coord.y + " out of universe");
			}
		}
		
		if( tryno == tries )
		{
			console.Print("Something went wrong, couldn't pick a capital for " + i);
			
			// total repick!
			return false;
		}
	}
	
	return true;
}

//--------------------------------------------------------------------------
// Name:
// Desc: special function to expand factions areas around picked capitals
//--------------------------------------------------------------------------
function GenerateFactions_CreateFactionsAreas( factions, start_system_id )
{
	var bGenerated;
	var bNeeded;
	var sti;
	var cursysfac;
	var i;
	var total_num_of_stars;
	var stars;
	
	// fill first system
	for(i in factions)
	{
		factions[i].stars = [ factions[i].capital ];
	}
	
	
	while(true)
	{
		bGenerated = false;
		bNeeded = false;
		
		// try to expand every faction by 1 unit (1 link)
		for(i in factions)
		{
			total_num_of_stars = factions[i].stars_count_value + NumOfSystems * (factions[i].stars_count_percent  / 100);
						
			
			// lack of stars - needs to be generated
			if( factions[i].stars.length < total_num_of_stars )
			{	
				bNeeded = true;
				
				//console.Print("trying to expand");
				stars = generator.ExpandArea( factions[i].stars );
				
				//console.Print("expanded to: " + stars.length);
				for(sti = 0; sti < total_num_of_stars && sti < stars.length; sti++)
				{
					cursysfac = relations.GetSystemFaction(stars[sti]);
					if(cursysfac.length == 0 && stars[sti] != start_system_id)
					{
						relations.SetSystemFaction(stars[sti],i);
						bGenerated = true;
						//console.Print(stars[sti] + " faction set to " + i);
					}
					else if (cursysfac != i || stars[sti] == start_system_id)
					{
						//console.Print(stars[sti] + " already taken");
						stars.splice(sti,1);
						sti--;
					}
				}
				for(sti = stars.length; sti > total_num_of_stars; sti--)
					stars.pop(); //remove all extra stars
					
				factions[i].stars = stars;
			}	
		}
		
		// if nothing was generated, it means either all systems properly generated,
		// or error occured, and nothing can be generated
		if(!bGenerated)
		{
			if(bNeeded) return false;
			else return true;
		}
	}
}







