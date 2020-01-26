using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class BalloonBreakSuccessTests : MonoBehaviour
    {
        private GameObject balloonObject;
        private NewBalloonController balloonController;
        private GameObject bounceCube;

        [SetUp]
        public void SetUp()
        {
            balloonObject = Instantiate(Resources.Load<GameObject>("Prefabs/Balloon"));
            balloonController = balloonObject.GetComponent<NewBalloonController>();

            Transform cubeTransform = new GameObject().transform;
            cubeTransform.position = new Vector3(0, 2, 0);
            bounceCube = Instantiate(Resources.Load<GameObject>("Prefabs/interactiveSquare"), cubeTransform);
        }

        [TearDown]
        public void TearDown()
        {
            var gameObjects = FindObjectsOfType(typeof(GameObject));

            foreach (var gameObject in gameObjects)
            {
                DestroyImmediate(gameObject);
            }
        }

        [UnityTest]
        public IEnumerator BalloonBreak_Type0_Success()
        {
            balloonController.balloonType = 0;
            balloonController.breakForce = 0.0f;
            bounceCube.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -5);

            Assert.AreEqual(balloonController.balloonType, 0);
            yield return new WaitForSeconds(1.5f);

            Assert.IsTrue(balloonObject == null);
        }

        [UnityTest]
        public IEnumerator BalloonBreak_Type1_Success()
        {
            balloonController.balloonType = 1;
            balloonController.breakForce = 0.0f;
            bounceCube.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -5);

            Assert.AreEqual(balloonController.balloonType, 1);
            yield return new WaitForSeconds(1.5f);

            Assert.IsTrue(balloonObject == null);
        }

        [UnityTest]
        public IEnumerator BalloonBreak_TypeMinus1_Success()
        {
            balloonController.balloonType = -1;
            balloonController.breakForce = 0.0f;
            bounceCube.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -5);

            Assert.AreEqual(balloonController.balloonType, -1);
            yield return new WaitForSeconds(1.5f);

            Assert.IsTrue(balloonObject == null);
        }

        [UnityTest]
        public IEnumerator Type0_NoEffect_Success()
        {
            balloonController.balloonType = 0;
            balloonController.breakForce = 0.0f;
            balloonController.bounciness = 0.0f;
            bounceCube.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            bounceCube.transform.position = new Vector3(0, -2, 0);

            Assert.AreEqual(balloonController.balloonType, 0);
            yield return new WaitForSeconds(0.1f);

            Assert.IsTrue(bounceCube.GetComponent<Rigidbody2D>().velocity.magnitude == 0.0f);
        }

        [UnityTest]
        public IEnumerator Type1_PushEffect_Success()
        {
            balloonController.balloonType = 1;
            balloonController.breakForce = 0.0f;
            balloonController.bounciness = 0.0f;
            balloonObject.transform.position = new Vector3(-0.5f, 0, 0);
            bounceCube.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            bounceCube.transform.position = new Vector3(0, -2, 0);

            Assert.AreEqual(balloonController.balloonType, 1);
            yield return new WaitForSeconds(0.5f);

            Assert.IsTrue(bounceCube.GetComponent<Rigidbody2D>().velocity.x > 0.0f);
        }

        [UnityTest]
        public IEnumerator TypeMinus1_PullEffect_Success()
        {
            balloonController.balloonType = -1;
            balloonController.breakForce = 0.0f;
            balloonController.bounciness = 0.0f;
            balloonObject.transform.position = new Vector3(0.5f, 0, 0);
            bounceCube.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            bounceCube.transform.position = new Vector3(0, -2, 0);

            Assert.AreEqual(balloonController.balloonType, -1);
            yield return new WaitForSeconds(0.5f);

            Assert.IsTrue(bounceCube.GetComponent<Rigidbody2D>().velocity.x > 0.0f);
        }

    }
}
