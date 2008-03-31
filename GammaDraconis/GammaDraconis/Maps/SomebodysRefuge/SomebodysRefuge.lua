gameScene = Engine.GetInstance().gameScene

p = Player(GammaDraconis, PlayerIndex.One);
gameScene:track(p, GO_TYPE.RACER);

p2 = Player(GammaDraconis, PlayerIndex.Two);
p2.position = Coords(20.0, -12.0, 28.0);
gameScene:track(p2, GO_TYPE.RACER);

p3 = Player(GammaDraconis, PlayerIndex.Three);
p3.position = Coords(20.0, 12.0, 28.0);
gameScene:track(p3, GO_TYPE.RACER);

p4 = Player(GammaDraconis, PlayerIndex.Four);
p4.position = Coords(20.0, 12.0, -28.0);
gameScene:track(p4, GO_TYPE.RACER);

r = Racer(GammaDraconis);
r.position = Coords(20.0, -12.0, -28.0);
r.models[0].scale = 2;
gameScene:track(r, GO_TYPE.RACER);

checkpoint = GameObject();
checkpoint.position = Coords(75, 0, -75, 0, 1.25 * MSMath.PI, 0);
checkpoint.models:Add(FBXModel("Resources/Models/Checkpoint", "", 10));
gameScene:track(checkpoint, GO_TYPE.HUD);

planet = GameObject();
planet.position = Coords(0, 0, -50);
planet.models:Add(FBXModel("Resources/Models/Planet", "", 50));
gameScene:track(planet, GO_TYPE.SCENERY);

skybox = GameObject();
skybox.models:Add(FBXModel("Resources/Models/Skybox", "", 500*10000));
gameScene:track(skybox, GO_TYPE.SKYBOX);
