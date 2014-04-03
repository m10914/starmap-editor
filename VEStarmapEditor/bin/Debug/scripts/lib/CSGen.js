//--------------------------------------------------------
// Cluster System Generation
// CSGen.js
//
// Script for VoidExpance (c) game
// Author: DayWall
//--------------------------------------------
// Desc:
//	This small library provides an opportunity to simplify
//	the process of creation of systems through scripts.
//
//

using(generator);

CSGen = {
	
	SystemTypes: ["generic", "lava", "ice"],

	GetSystemTypeByRandom : function(num)
	{
		var rnum = num % this.SystemTypes.Length;
		return this.SystemTypes[rnum];
	},
	
	
	//----------------------------------------------------
	// G E N E R I C   S Y S T E M
	//
	
	//creates system
	CreateGenericSystem : function( args )
	{
		this.CreateGenericStars(args);
		this.CreateGenericPlanets(args);
		this.CreateGenericAsteroids(args);
		this.CreateGenericObjects(args);
	},
	
	// creates stars
	CreateGenericStars : function( args )
	{
		generator.AddStar(
			args.sys_id, //system
			0, //coordinateX
			0, //coordinateY
			MathExt.RandInt(), //seed
			100, //heat
			"sun"); //type
		
		if(MathExt.RandDouble() > 0.9)
		{
			//generate twin
			generator.AddStar(args.sys_id,
				MathExt.RandDouble()*30+20,
				MathExt.RandDouble()*30+20,
				MathExt.RandInt(),
				0,
				"sun_small");
		}
	},
	
	//create planets
	CreateGenericPlanets: function( args )
	{
		var bWasBelt = false;
		
		//maximum 8, 0 is VERY RARE occasion, mostly over 3
		var numOfPlanets = MathExt.RandInt()%3 + MathExt.RandInt()%3 + MathExt.RandInt()%2;
		var curRadius = MathExt.RandInt() % 30 + 80;
		for(j=0; j < numOfPlanets; j++)
		{
			//roll for belt
			if(!bWasBelt && MathExt.RandInt() % 10 == 0)
			{
				bWasBelt = true;
				this.CreateBeltOfType( args, curRadius, this.GetGenericAsteroidTypes() );
			}
			else
			{
				var ang = MathExt.RandDouble() * 3.1415 * 2.0;
				generator.AddPlanet(
					args.sys_id,
					curRadius * Math.cos(ang),
					curRadius * Math.sin(ang),
					"GeneratedPlanetName",
					MathExt.RandInt());
			}
			curRadius += MathExt.RandInt() % 30 + 80;
		}
	},
	
	
	//create asteroids
	CreateGenericAsteroids: function ( args )
	{
		var asteroidTypesGeneric = this.GetGenericAsteroidTypes(args);
		var bounds = this.GetWorldBounds( args );
		
		//create clusters
		var density = 40;
		var num = Math.round( bounds.x / density );
		var intensity = MathExt.RandInt()%5 + 4;
		for(var j=-num; j < num; j++)
			for(var i = -num; i < num; i++)
			{
				if(MathExt.RandInt() % intensity == 0)
					this.CreateClusterOfType( args, asteroidTypesGeneric, i, j, density );
			}
				
		//create fields
		var FieldCount = MathExt.RandInt() % 3 + 1;
		for(var j = 0; j < FieldCount; j++)
		{
			this.CreateFieldOfType( args, asteroidTypesGeneric);
		}
	},
	
	GetGenericAsteroidTypes : function ( args )
	{
		var asteroidTypesAll = generator.GetAsteroidTypes();
		var asteroidTypesGeneric = [];
		//filter only generic ones
		for(var i=0; i < asteroidTypesAll.length; i++)
		{
			if(asteroidTypesAll[i].search("rock") > 0)
				asteroidTypesGeneric.push(asteroidTypesAll[i]);
		}
		
		return asteroidTypesGeneric;
	},
	
	//create objects (like debris)
	CreateGenericObjects: function ( args )
	{
	},
	
	//
	// eof: generic system
	//----------------------------------------------------


	
	
	//--------------------------------------------------------------
	// Name:
	// Desc: creates a cluster of asteroids of chosen type
	//--------------------------------------------------------------
	CreateClusterOfType : function( args, types, x_pos, y_pos, mult )
	{
		var NumAsteroids = 1;
		if(MathExt.RandInt() % 10 == 0)
			NumAsteroids = MathExt.RandInt()%3 + 3;
		var CoordOfCluster = 
		{
			x: (x_pos + MathExt.RandDouble()) * mult, 
			y: (y_pos + MathExt.RandDouble()) * mult, 
		};
		
		for(j=0; j < NumAsteroids; j++)
		{
			var crd_x = CoordOfCluster.x;
			var crd_y = CoordOfCluster.y;
			var asteroidTypeId = types[MathExt.RandRange(0, types.length)];
			var scale = MathExt.RandDouble() * 0.4 + 0.5;
			var resQ = 1.0;
			
			var rotation = { X: (MathExt.RandDouble()) * 90, Y: (MathExt.RandDouble()) * 90, Z: 0.0 };
			var rotationSpeed = { X: (MathExt.RandDouble() * 0.5 - 1.0) * 0.25 * 90, Y: 0.0, Z: 0.0 };
			
			var ast_id = 
				generator.AddAsteroid(
					args.sys_id,
					crd_x, crd_y,
					asteroidTypeId,
					scale,
					resQ,
					rotation,
					rotationSpeed);
		}
	},

	//--------------------------------------------------------------
	// Name:
	// Desc: creates a field of asteroids of chosen type
	//--------------------------------------------------------------
	CreateFieldOfType : function( args, types )
	{
		var bounds = this.GetWorldBounds( args );
		var NumAsteroids = MathExt.RandInt()%30 + 7;
		
		var FieldAng = MathExt.RandDouble() * 3.1415 * 2.0;
		var FieldDistance = MathExt.RandInt() % (bounds.x - 200) + 200; //from 200 to bound
		
		var CurAng = FieldAng;
		for(j=0; j < NumAsteroids; j++)
		{
			var angOffset = (MathExt.RandDouble() * 14 + 8) / FieldDistance;
			var newDist = FieldDistance + MathExt.RandDouble() * 40.0 - 20;
			CurAng += angOffset;
		
			var crd_x = newDist * Math.cos(CurAng);
			var crd_y = newDist * Math.sin(CurAng);
			
			var asteroidTypeId = types[MathExt.RandRange(0, types.length)];
			var scale = MathExt.RandDouble() * 0.4 + 0.5;
			var resQ = 1.0;
			
			var rotation = { X: (MathExt.RandDouble()) * 90, Y: (MathExt.RandDouble()) * 90, Z: 0.0 };
			var rotationSpeed = { X: (MathExt.RandDouble() * 0.5 - 1.0) * 0.25 * 90, Y: 0.0, Z: 0.0 };
			
			var ast_id = 
				generator.AddAsteroid(
					args.sys_id,
					crd_x, crd_y,
					asteroidTypeId,
					scale,
					resQ,
					rotation,
					rotationSpeed);
		}
	},

	//--------------------------------------------------------------
	// Name:
	// Desc: creates a belt of asteroids of chosen type
	//--------------------------------------------------------------
	CreateBeltOfType: function( args, dist, types )
	{
		var CurAng = 0;
		while(CurAng < 3.1415 * 2.0)
		{
			var angOffset = (MathExt.RandDouble() * 14 + 8) / dist;
			var newDist = dist + MathExt.RandDouble() * 40.0 - 20;
			CurAng += angOffset;
		
			var crd_x = newDist * Math.cos(CurAng);
			var crd_y = newDist * Math.sin(CurAng);
			
			var asteroidTypeId = types[MathExt.RandRange(0, types.length)];
			var scale = MathExt.RandDouble() * 0.4 + 0.5;
			var resQ = 1.0;
			
			var rotation = { X: (MathExt.RandDouble()) * 90, Y: (MathExt.RandDouble()) * 90, Z: 0.0 };
			var rotationSpeed = { X: (MathExt.RandDouble() * 0.5 - 1.0) * 0.25 * 90, Y: 0.0, Z: 0.0 };
			
			var ast_id = 
				generator.AddAsteroid(
					args.sys_id,
					crd_x, crd_y,
					asteroidTypeId,
					scale,
					resQ,
					rotation,
					rotationSpeed);
		}
	},
	
	
	//some helpful generator functions
	GetWorldBounds : function ( args )
	{
		return {x: 350, y: 350};
	},
	GetRandomCoordWithinWorldBounds: function ( args )
	{
		var bounds = this.GetWorldBounds( args );
		return {
			x: MathExt.RandInt() % (bounds.x * 2) - bounds.x,
			y: MathExt.RandInt() % (bounds.y * 2) - bounds.y,
			};
	},
};
