gameScene = Engine.GetInstance().gameScene
course = Engine.GetInstance().course

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
r.position = Coords(0.0, 0.0, 0.0);
r.models[0].scale = 2;
gameScene:track(r, GO_TYPE.RACER);

--[[
checkpoint = GameObject();
checkpoint.position = Coords(75, 0, -75, 0, 1.25 * MSMath.PI, 0);
checkpoint.models:Add(FBXModel("Resources/Models/Checkpoint", "", 10));
gameScene:track(checkpoint, GO_TYPE.HUD);
]]--

planet = GameObject();
planet.position = Coords(0, 0, -50);
planet.models:Add(FBXModel("Resources/Models/Planet", "", 50));
gameScene:track(planet, GO_TYPE.SCENERY);

skybox = GameObject();
skybox.models:Add(FBXModel("Resources/Models/Skybox", "", 500*10000));
gameScene:track(skybox, GO_TYPE.SKYBOX);


path = {
	{x=0, y=0, z=50, yaw=0, pitch=0, roll=0 },
	{x=0, y=0, z=100, yaw=0.25, pitch=0, roll=1.5, path=true},
	{x=0, y=0, z=200, yaw=0.5, pitch=0, roll=0},
	{x=50, y=0, z=400, yaw=1.0, pitch=-0.3, roll=0},
	{x=125, y=50, z=500, yaw=1.0, pitch=-0.4, roll=0},
	{x=200, y=125, z=600, yaw=1.0, pitch=-0.5, roll=0},
	{x=275, y=200, z=700, yaw=1.0, pitch=-0.4, roll=0},
	{x=350, y=275, z=800, yaw=1.0, pitch=-0.2, roll=0},
	{x=425, y=325, z=900, yaw=1.0, pitch=0.2, roll=0},
	{x=575, y=200, z=1100, yaw=1.0, pitch=0.5, roll=0},
	}


for i,v in ipairs( path ) do
	local position = Coords( v.x, v.y, v.z, v.pitch, v.yaw, v.roll)
	course.path:Add(position);

	if not v.path then
		checkpoint = GameObject();
		checkpoint.position = position;
		checkpoint.models:Add(FBXModel("Resources/Models/Checkpoint", "", 10));
		gameScene:track(checkpoint, GO_TYPE.HUD);
	end
end