--[[ 
  Asteroid Definitions 
  ----
  Asteroid800A
  ]]--
  
-- AsteroidBase
base = GameObject()
base.dragR = 0
base.dragL = 0
base.mass = 100000
base.size = 100
base.maxHealth = 3000

function asteroidDead(asteroid)
	Engine.GetInstance().gameScene:ignore(asteroid, GO_TYPE.DEBRIS)
	if asteroid.maxHealth > base.maxHealth / 8 then
		local oldPos = asteroid.position:pos()
		local velocity = Coords( Matrix.Multiply( asteroid.velocity:matrix(), Matrix.CreateScale( Vector3( 0.25 ) ) ).Translation )
		velocity.R = Engine.GetInstance():qScale( asteroid.velocity.R, 0.5 )
		
		local roid1 = asteroid:clone()
		roid1.size = asteroid.size / 2
		roid1:scaleModels(0.35)
		roid1.mass = asteroid.mass / 2
		roid1.maxHealth = asteroid.maxHealth / 2
		roid1.velocity = velocity
		local roid2 = roid1:clone()
		roid2.velocity = velocity
		local roid3 = roid1:clone()
		roid3.velocity = velocity
		local roid4 = roid1:clone()
		roid4.velocity = velocity
		local roid5 = roid1:clone()
		roid5.velocity = velocity
		local roid6 = roid1:clone()
		roid6.velocity = velocity
		local roid7 = roid1:clone()
		roid7.velocity = velocity
		local roid8 = roid1:clone()
		roid8.velocity = velocity
		local roid9 = roid1:clone()
		roid9.velocity = velocity
		local roidA = roid1:clone()
		roidA.velocity = velocity
		local roidB = roid1:clone()
		roidB.velocity = velocity
		local roidC = roid1:clone()
		roidC.velocity = velocity
		local roidD = roid1:clone()
		roidD.velocity = velocity
		local roidE = roid1:clone()
		roidE.velocity = velocity
		local offset = roid1.size * 1.2
		roid1.position = Coords(oldPos.X + offset, oldPos.Y + offset, oldPos.Z + offset)
		roid2.position = Coords(oldPos.X + offset, oldPos.Y + offset, oldPos.Z - offset)
		roid3.position = Coords(oldPos.X + offset, oldPos.Y - offset, oldPos.Z + offset)
		roid4.position = Coords(oldPos.X + offset, oldPos.Y - offset, oldPos.Z - offset)
		roid5.position = Coords(oldPos.X - offset, oldPos.Y + offset, oldPos.Z + offset)
		roid6.position = Coords(oldPos.X - offset, oldPos.Y + offset, oldPos.Z - offset)
		roid7.position = Coords(oldPos.X - offset, oldPos.Y - offset, oldPos.Z + offset)
		roid8.position = Coords(oldPos.X - offset, oldPos.Y - offset, oldPos.Z - offset)
		offset = offset * 2
		roid9.position = Coords(oldPos.X + offset, oldPos.Y, oldPos.Z)
		roidA.position = Coords(oldPos.X - offset, oldPos.Y, oldPos.Z)
		roidB.position = Coords(oldPos.X, oldPos.Y + offset, oldPos.Z)
		roidC.position = Coords(oldPos.X, oldPos.Y - offset, oldPos.Z)
		roidD.position = Coords(oldPos.X, oldPos.Y, oldPos.Z + offset)
		roidE.position = Coords(oldPos.X, oldPos.Y, oldPos.Z - offset)
		local spawnChance = 0.6
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid1, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid2, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid3, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid4, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid5, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid6, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid7, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid8, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roid9, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roidA, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roidB, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roidC, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roidD, GO_TYPE.DEBRIS)
		end
		if math.random() < spawnChance then
			Engine.GetInstance().gameScene:track(roidE, GO_TYPE.DEBRIS)
		end
	end
end

base.OnDeathFunction = GammaDraconis.GameLua:GetFunction("asteroidDead")

-- Asteroid800A
fbx = FBXModel("Resources/Models/Asteroid800A", "", 1)

roid = base:clone()
roid.models:Add(fbx)

Proto.thing:Add("Asteroid800A", roid)

-- Asteroid800B
fbx = FBXModel("Resources/Models/Asteroid800B", "", 1)

roid = base:clone()
roid.models:Add(fbx)

Proto.thing:Add("Asteroid800B", roid)
