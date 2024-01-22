using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Riptide;
using UnityEngine;
using UnityEngine.TestTools;

public class Tester
{
    // A Test behaves as an ordinary method
    [Test]
    public void TesterSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TesterWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestConnection()
    {
        NetworkManager network = MonoBehaviour.Instantiate<NetworkManager>(Resources.Load<NetworkManager>("NetworkManager"));
        Assert.NotNull(network);

        MultiEnemySpawn esm = MonoBehaviour.Instantiate<MultiEnemySpawn>(Resources.Load<MultiEnemySpawn>("EnemySpawnManager"));
        Assert.NotNull(esm);

        yield return null;

        Assert.AreEqual(esm.EnemyCount(), 0);

        while (network.isDisconnected())
        {
            yield return null;
        }

        Message connect = Message.Create(MessageSendMode.Reliable, (ushort)(ClientToServerId.joinSession));
        connect.AddUShort((ushort)SessionType.pub);
        network.Client.Send(connect);
        
        while (!network.isInSession())
        {
            yield return null;
        }

        Debug.Log("(CLIENT): Entered session.");

        while (esm.EnemyCount() == 0)
        {
            yield return null;
        }

        Debug.Log("(ENEMY_MANAGER): Spawned enemy.");

        network.Client.Disconnect();

        yield return null;
    }
}
