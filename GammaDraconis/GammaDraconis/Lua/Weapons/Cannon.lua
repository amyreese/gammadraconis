
weapon = Weapon()

weapon.bullet.damage = 400
weapon.bullet.rateL = 800
weapon.bullet.models:Add(FBXModel("Resources/Models/Shell", "", 1.0));

Proto.weapon:Add("Cannon", weapon)