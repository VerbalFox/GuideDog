using Godot;
using System;
using System.Net;

public class Menu : Control
{
    private Button hostButton, joinButton;
    private TextEdit ipTextBox;
    private Control lobby;
    private Label ipLabel, timeLabel, lobbyStatusLabel;
    private NetworkManager networkManager;
    private CheckBox playerReadyBox;
    private string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        networkManager = GetNode<NetworkManager>("/root/root/NetworkManager");
        hostButton = GetNode<Button>("Host");
        joinButton = GetNode<Button>("Join");
        ipTextBox = GetNode<TextEdit>("IpAddressBox");
        lobby = GetNode<Control>("Lobby");
        ipLabel = GetNode<Label>("IpAddressLabel");
        timeLabel = GetNode<Label>("TimeLabel");
        lobbyStatusLabel = GetNode<Label>("Lobby/LobbyStatus");

        playerReadyBox = GetNode<CheckBox>("Lobby/PlayerReady");

        hostButton.Connect("pressed", this, "Host");
        joinButton.Connect("pressed", this, "Join");
        playerReadyBox.Connect("pressed", this, "Ready");

        ipLabel.Text = externalIpString;
    }

    private void Host() {
        networkManager.OpenSocket(24010);

        networkManager.isHost = true;
    }

    private void Join() {
        networkManager.OpenSocket(24011);
        networkManager.Connect("127.0.0.1", 24010);
        networkManager.SendConnectionRequest();
        
        lobby.Visible = true;
    }

    private void Ready() {
        networkManager.isReady = !networkManager.isReady;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timeLabel.Text = networkManager.timeElapsed.ToString();
        
        if (networkManager.connected) {
            ipLabel.Text = "Connected!!";
            lobby.Visible = true;

            if (networkManager.isHost) {
                lobbyStatusLabel.Text = $"You: {networkManager.isReady} \nPlayer 2: {networkManager.isRemoteClientReady}";
            } else {
                lobbyStatusLabel.Text = $"Player 1: {networkManager.isRemoteClientReady} \nYou: {networkManager.isReady}";
            }

            if (networkManager.gameStarting) {
                lobbyStatusLabel.Text += "\n\nGame starting in 5 seconds.";
            }

        } else {
            ipLabel.Text = externalIpString;
        }


    }
}
