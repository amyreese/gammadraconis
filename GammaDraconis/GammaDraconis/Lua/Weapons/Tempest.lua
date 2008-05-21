
weapon = Weapon()
weapon.type = W_TYPE.SECONDARY
weapon.cooldown = 400
weapon.ammoMax = 15
weapon.fireFrom = Coords(0,-7,0)
weapon.fireSFX = "fire_missile"
weapon.impactSFX = "explosion"
weapon.bullet.onDeathSound = "explosion"
weapon.bullet.explosion = Explosion()

weapon.bullet.damage = 150
weapon.bullet.rateL = 1000
weapon.bullet.dragL = 0
weapon.bullet.dragR = 0.5
weapon.bullet.size = 20
weapon.bullet.timeToLive = 3
weapon.bullet.models:Add(FBXModel("Resources/Models/Rocket", "", 1.0));
weapon.bullet.models:Add(FBXModel("Resources/Models/Rocket2", "", 1.0));

Proto.weapon:Add("Tempest", weapon)