#pragma strict

public var sillyName : String = "blah blah";

public var forceLevel : float = 2;

function Start () {
	rigidbody.AddForce(Random.onUnitSphere * forceLevel);
}

function Update () {

}