using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class BalloonSoundSuccessTests : MonoBehaviour
    {
        private GameObject balloonObject;
        private NewBalloonController balloonController;
        private GameObject bounceCube;

        [SetUp]
        public void SetUp()
        {
            balloonObject = Instantiate(Resources.Load<GameObject>("Prefabs/Balloon"));
            balloonObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            balloonController = balloonObject.GetComponent<NewBalloonController>();

            Transform cubeTransform = new GameObject().transform;
            cubeTransform.position = new Vector3(0, 2, 0);
            bounceCube = Instantiate(Resources.Load<GameObject>("Prefabs/interactiveSquare"), cubeTransform);
            bounceCube.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -5);
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
        public IEnumerator BounceSound_Success()
        {
            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(FindObjectOfType<AudioSource>().clip.name, "bounce");
        }

        [UnityTest]
        public IEnumerator PopSound_BalloonType1_Success()
        {
            balloonController.breakForce = 0.0f;
            balloonController.balloonType = 1;

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(FindObjectOfType<AudioSource>().clip.name, "pop");
        }

        [UnityTest]
        public IEnumerator PopSound_BalloonType0_Success()
        {
            balloonController.breakForce = 0.0f;
            balloonController.balloonType = 0;

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(FindObjectOfType<AudioSource>().clip.name, "pop");
        }

        [UnityTest]
        public IEnumerator ImplodeSound_BalloonTypeMinus1_Success()
        {
            balloonController.breakForce = 0.0f;
            balloonController.balloonType = -1;

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(FindObjectOfType<AudioSource>().clip.name, "implode");
        }
    }
}
