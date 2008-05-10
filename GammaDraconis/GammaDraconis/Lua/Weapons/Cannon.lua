
weapon = Weapon()
weapon.cooldown = 200

weapon.bullet.damage = 500
weapon.bullet.rateL = 750
weapon.bullet.timeToLive = 8
weapon.bullet.models:Add(FBXModel("Resources/Models/Shell", "", 1.0));

Proto.weapon:Add("Cannon", weapon)