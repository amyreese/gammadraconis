
function makeRoidRow(xoffset, yoffset, zoffset, gameScene)
	roid = Proto.getThing("Asteroid800A", Coords(xoffset-2500,yoffset,zoffset), Coords(0.3 * math.random(),-0.2 * math.random(),0,0.004,0.001,0.0004))
	gameScene:track(roid, GO_TYPE.DEBRIS)

	roid = Proto.getThing("Asteroid800A", Coords(xoffset-1500,yoffset,zoffset), Coords(0.6 * math.random(),-0.4 * math.random(),0,0.004,0.001,0.0004))
	gameScene:track(roid, GO_TYPE.DEBRIS)

	roid = Proto.getThing("Asteroid800A", Coords(xoffset-500,yoffset,zoffset), Coords(0.01 * math.random(),-0.1 * math.random(),0,0.004,0.001,0.0004))
	gameScene:track(roid, GO_TYPE.DEBRIS)

	roid = Proto.getThing("Asteroid800A", Coords(xoffset+500,yoffset,zoffset), Coords(-0.6 * math.random(),0.4 * math.random(),0,0.004,0.001,0.0004))
	gameScene:track(roid, GO_TYPE.DEBRIS)

	roid = Proto.getThing("Asteroid800A", Coords(xoffset+1500,yoffset,zoffset), Coords(-0.3 * math.random(),0.2 * math.random(),0,0.004,0.001,0.0004))
	gameScene:track(roid, GO_TYPE.DEBRIS)
end


function makeRoidPlane(xoffset, yoffset, zoffset, gameScene, columns)
	for x=0,columns do
		makeRoidRow(xoffset, yoffset, zoffset + (500 * x), gameScene)
	end
end

function makeRoidCube(xoffset, yoffset, zoffset, gameScene, planes)
	for x=0, planes do
		makeRoidPlane(xoffset, yoffset + (500 * x), zoffset, gameScene, planes)
	end
end