let dialogGrid = document.getElementsByClassName("main-grid")[0];

function GetDialogById(dialogs, id)
{
	for(let i = 0; i < dialogs.length; i++)
	{
		let dialogId = dialogs[i].getAttribute("dialogid");
		dialogId = parseInt(dialogId);

		if(dialogId == id)
			return dialogs[i];
	}
}

function RemoveFirstDialogById(dialogs, id)
{
	for(let i = 0; i < dialogs.length; i++)
	{
		let dialogId = dialogs[i].getAttribute("dialogid");
		dialogId = parseInt(dialogId);

		if(dialogId == id)
		{
			let header = dialogs[i].getElementsByClassName("dialog-header")[0];
			header.className = "dialog-header dialog-done";
			//dialogs[i].removeEventListener('click', dialogClick);
			return;
		}
	}
}

function CreateNewDialogs(id)
{
	let url = urlBase + "dialog/view/" + id;
	let method = "GET";
	sendAjax(url, method, function (result)
	{
		var element = $(result)[0];
		GetMoreTimeOnClick(element);
		$("#main-grid").append(element);
	});
}

function UpdateDialog(id, dialogs)
{
	let url = urlBase + "dialog/view/" + id;
	let method = "GET";
	sendAjax(url, method, function (result)
	{
		var element = $(result)[0];
		GetMoreTimeOnClick(element);

		let dialog = GetDialogById(dialogs, id)
		dialog.removeEventListener('click', dialogClick);
		dialog.replaceWith(element);
	});
}



const hubConnection = new signalR
	.HubConnectionBuilder()
    .withUrl("/signalr", { transport: signalR.HttpTransportType.WebSockets })
    .build();

hubConnection.on("Delete", function (id) {
	id = JSON.stringify(id);
	id = JSON.parse(id);
	console.log(id);
	let dialogs = document.getElementsByClassName("dialog");
	RemoveFirstDialogById(dialogs, id);
});

hubConnection.on("Create", function (id) {
	id = JSON.stringify(id);
	id = JSON.parse(id);
	CreateNewDialogs(id)
});

hubConnection.on("Update", function (id) {
	let dialogs = document.getElementsByClassName("dialog");
	id = JSON.stringify(id);
	id = JSON.parse(id);
	UpdateDialog(id, dialogs);
});

hubConnection.on("DeleteAll", function (id) {
	let dialogs = document.getElementsByClassName("dialog");
	for(let i = 0; i < dialogs.length; i++)
	{
		let dialogId = dialogs[i].getAttribute("dialogid");
		dialogId = parseInt(dialogId);

		RemoveFirstDialogById(dialogs, dialogId);
	}
});

hubConnection.start();