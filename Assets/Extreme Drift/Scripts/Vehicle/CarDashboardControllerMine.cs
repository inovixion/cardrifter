using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarDashboardControllerMine : MonoBehaviour
{
	private VehicleControlDaim carController;
	[System.Serializable]
	public class RPMDial
	{

		public GameObject dial;
		public float multiplier = .05f;
		public RotateAround rotateAround = RotateAround.Z;
		private Quaternion dialOrgRotation = Quaternion.identity;
		public Text text;

		public void Init()
		{

			if (dial)
				dialOrgRotation = dial.transform.localRotation;

		}

		public void Update(float value)
		{

			Vector3 targetAxis = Vector3.forward;

			switch (rotateAround)
			{

				case RotateAround.X:

					targetAxis = Vector3.right;

					break;

				case RotateAround.Y:

					targetAxis = Vector3.up;

					break;

				case RotateAround.Z:

					targetAxis = Vector3.forward;

					break;

			}

			dial.transform.localRotation = dialOrgRotation * Quaternion.AngleAxis(-multiplier * value, targetAxis);
			if (text)
				text.text = value.ToString("F0");

		}

	}

	[System.Serializable]
	public class SpeedoMeterDial
	{

		public GameObject dial;
		public float multiplier = 1f;
		public RotateAround rotateAround = RotateAround.Z;
		private Quaternion dialOrgRotation = Quaternion.identity;
		public Text text;

		public void Init()
		{

			if (dial)
				dialOrgRotation = dial.transform.localRotation;

		}

		public void Update(float value)
		{

			Vector3 targetAxis = Vector3.forward;

			switch (rotateAround)
			{

				case RotateAround.X:

					targetAxis = Vector3.right;

					break;

				case RotateAround.Y:

					targetAxis = Vector3.up;

					break;

				case RotateAround.Z:

					targetAxis = Vector3.forward;

					break;

			}

			dial.transform.localRotation = dialOrgRotation * Quaternion.AngleAxis(-multiplier * value, targetAxis);

			if (text)
				text.text = value.ToString("F0");

		}

	}

	[Space()]
	public RPMDial rPMDial;
	[Space()]
	public SpeedoMeterDial speedDial;

	public enum RotateAround { X, Y, Z }

	void Awake()
	{

		carController = GetComponentInParent<VehicleControlDaim>();

		rPMDial.Init();
		speedDial.Init();

	}

	void Update()
	{

		if (!carController)
			return;

		Dials();

	}

	void Dials()
	{

		if (rPMDial.dial != null)
			rPMDial.Update(carController.motorRPM);

		if (speedDial.dial != null)
			speedDial.Update(carController.speed);

	}
}
