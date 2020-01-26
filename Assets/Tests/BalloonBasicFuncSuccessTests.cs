using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class BalloonBasicFuncSuccessTests : MonoBehaviour
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
            bounceCube.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -10);
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
        public IEnumerator ObjectBounce_Success()
        {
            balloonController.bounciness = 0.5f;
            balloonObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

            Vector2 veloBefore = bounceCube.GetComponent<Rigidbody2D>().velocity;
            yield return new WaitForSeconds(0.1f);

            Assert.Greater(bounceCube.GetComponent<Rigidbody2D>().velocity.y, veloBefore.y);
        }


        [UnityTest]
        public IEnumerator ObjectBounceHarder_Success()
        {
            balloonController.bounciness = 5.0f;
            balloonObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

            Vector2 veloBefore = bounceCube.GetComponent<Rigidbody2D>().velocity;
            yield return new WaitForSeconds(0.1f);

            Assert.Greater(Mathf.Abs(bounceCube.GetComponent<Rigidbody2D>().velocity.y), Mathf.Abs(veloBefore.y));
        }

        [UnityTest]
        public IEnumerator BalloonBounceBack_Success()
        {
            bounceCube.transform.position = new Vector3(0, -2.0f, 0);
            bounceCube.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 10.0f, 0);

            Vector2 veloBefore = balloonObject.GetComponent<Rigidbody2D>().velocity;
            yield return new WaitForSeconds(0.1f);

            Assert.Greater(balloonObject.GetComponent<Rigidbody2D>().velocity.y, veloBefore.y);
        }
    }
}
