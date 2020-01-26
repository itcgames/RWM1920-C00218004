using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class BalloonAnchorSuccessTests : MonoBehaviour
    {
        private GameObject balloonObject;
        private NewBalloonController balloonController;
        //private GameObject bounceCube;

        [SetUp]
        public void SetUp()
        {
            balloonObject = Instantiate(Resources.Load<GameObject>("Prefabs/Balloon"));
            balloonController = balloonObject.GetComponent<NewBalloonController>();

            //bounceCube = Instantiate(Resources.Load<GameObject>("Prefabs/interactiveSquare"));
            //Transform cubeTransform = new GameObject().transform;
            //cubeTransform.position = new Vector3(0, 2, 0);
            //bounceCube = Instantiate(Resources.Load<GameObject>("Prefabs/interactiveSquare"), cubeTransform);
            //bounceCube.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -10);
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
        public IEnumerator StaticAnchor_Success()
        {
            GameObject staticAnchor = Instantiate(Resources.Load<GameObject>("Prefabs/Anchor"));

            balloonController.anchorDistance = 3.0f;
            balloonController.SetAnchor(staticAnchor);

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(balloonController.anchorPoint, staticAnchor);
        }

        [UnityTest]
        public IEnumerator StaticAnchorNewDistance_Success()
        {
            GameObject staticAnchor = Instantiate(Resources.Load<GameObject>("Prefabs/Anchor"));

            balloonController.anchorDistance = 3.0f;
            balloonController.SetAnchor(staticAnchor, 5.0f);

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(balloonController.anchorPoint, staticAnchor);
            Assert.IsTrue(5.0f == balloonController.anchorDistance);
        }

        [UnityTest]
        public IEnumerator DynamicAnchor_Success()
        {
            GameObject dynamicAnchor = Instantiate(Resources.Load<GameObject>("Prefabs/interactiveSquare"));

            balloonController.anchorDistance = 3.0f;
            balloonController.SetAnchor(dynamicAnchor, 5.0f);

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(balloonController.spring.connectedBody, dynamicAnchor.GetComponent<Rigidbody2D>());
        }

        [UnityTest]
        public IEnumerator DynamicAnchorNewDistance_Success()
        {
            GameObject dynamicAnchor = Instantiate(Resources.Load<GameObject>("Prefabs/interactiveSquare"));

            balloonController.anchorDistance = 3.0f;
            balloonController.SetAnchor(dynamicAnchor, 5.0f);

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(balloonController.spring.connectedBody, dynamicAnchor.GetComponent<Rigidbody2D>());
            Assert.IsTrue(5.0f == balloonController.anchorDistance);
        }

        [UnityTest]
        public IEnumerator StaticAnchorLineBreak_Success()
        {
            GameObject staticAnchor = Instantiate(Resources.Load<GameObject>("Prefabs/Anchor"));
            staticAnchor.transform.position = new Vector2(10.0f, 10.0f);

            balloonController.spring.breakForce = 0.0f;
            balloonController.anchorDistance = 1.0f;
            balloonController.SetAnchor(staticAnchor);

            yield return new WaitForSeconds(0.1f);

            Assert.IsTrue(balloonController.spring == null);
        }

        [UnityTest]
        public IEnumerator DynamicAnchorLineBreak_Success()
        {
            GameObject dynamicAnchor = Instantiate(Resources.Load<GameObject>("Prefabs/interactiveSquare"));
            dynamicAnchor.transform.position = new Vector2(10.0f, 10.0f);

            balloonController.spring.breakForce = 0.0f;
            balloonController.anchorDistance = 1.0f;
            balloonController.SetAnchor(dynamicAnchor);

            yield return new WaitForSeconds(0.1f);

            Assert.IsTrue(balloonController.spring == null);
        }

    }
}
