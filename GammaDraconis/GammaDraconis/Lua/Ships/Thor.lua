
shipModel = FBXModel("Resources/Models/Thor", "", 1.25)
shipModel2 = FBXModel("Resources/Models/Thor2", "", 1.25)
shipModel2.lighted = false
shieldModel = FBXModel("Resources/Models/Shield", "", 0.25)
shieldModel.visible = false

mountR = MountPoint()
mountR.type = W_TYPE.PRIMARY
mountR.location = Coords(3.0, 0, 0)
mountR.weapon = Proto.getWeapon("Cannon")

mountL = MountPoint()
mountL.type = W_TYPE.PRIMARY
mountL.location = Coords(-3.0, 0, 0)
mountL.weapon = Proto.getWeapon("Cannon")

mountC = MountPoint()
mountC.type = W_TYPE.SECONDARY
mountC.location = Coords(0, -1, 0)
mountC.weapon = Proto.getWeapon("Tempest")

ship = GameObject()

ship.mass = 800

ship.size = 20

ship.rateL = 8.55
ship.dragL = 1

ship.rateR = 5
ship.dragR = 2.5

ship.relativeLookAt = Vector3(0,0,-300)
ship.relativeLookFrom = Vector3(0,10,48)

ship.maxHealth = 500;
ship.maxShield = 200;
ship.shieldIncreaseRate = 20;

ship.models:Add(shipModel)
ship.models:Add(shipModel2)
ship.shieldModel = shieldModel

ship.mounts:Add(mountR)
ship.mounts:Add(mountL)
ship.mounts:Add(mountC)

ship.thrusterSFX = "thor_thruster"
ship.engine_startSFX = "thor_engine_start"
ship.onDeathSound = "crash"

Proto.ship:Add("Thor", ship)
