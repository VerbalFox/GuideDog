using Godot;
using System;
using System.Net;

public class Menu : Control
{
    private Button hostButton, joinButton;
    private TextEdit ipTextBox;
    private Control lobby;
    private Label ipLabel, timeLabel;
    private NetworkManager networkManager;
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

        hostButton.Connect("pressed", this, "Host");
        joinButton.Connect("pressed", this, "Join");

        ipLabel.Text = externalIpString;
    }

    private void Host() {
        networkManager.OpenSocket(24010);
    }

    private void Join() {
        networkManager.OpenSocket(24011);
        networkManager.Connect(ipTextBox.Text, 24010);
        networkManager.SendConnectionRequest();
        
        lobby.Visible = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timeLabel.Text = networkManager.timeElapsed.ToString();

        if (networkManager.connected) {
            ipLabel.Text = "Connected!!";
            lobby.Visible = true;
        } else {
            ipLabel.Text = externalIpString;
        }
    }
}
