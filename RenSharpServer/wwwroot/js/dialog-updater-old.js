
let dialogGrid = document.getElementsByClassName("main-grid")[0];

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
			dialogs[i].removeEventListener('click', dialogClick);
			return;
		}
	}
}

function DeleteAcceptedDialogs(dialogs, ids)
{
	let dialogIds = [];

	for(let i = 0; i < dialogs.length; i++)
	{
		let header = dialogs[i].getElementsByClassName("dialog-header")[0];
		if(header.classList.contains("dialog-done"))
			continue;

		let dialogId = dialogs[i].getAttribute("dialogid");
		dialogId = parseInt(dialogId);
		dialogIds.push(dialogId);

		if(ids.includes(dialogId) == false)
		{
			RemoveFirstDialogById(dialogs, dialogId);
		}
	}

	return dialogIds;
}

function CreateNewDialogs(clientIds, serverIds)
{
	for(let i = 0; i < serverIds.length; i++)
	{
		// If server have ids that client doesnt have
		if(clientIds.includes(serverIds[i]) == false)
		{
			console.log(clientIds + " does not contains " + serverIds[i]);
			let url = urlBase + "dialog/view/" + serverIds[i];
			let method = "GET";
			sendAjax(url, method, function (result)
			{
				var element = $(result)[0];
				GetMoreTimeOnClick(element);
				$("#main-grid").append(element);
			});
		}
	}
}

function UpdateDialogs(serverIds)
{
	let dialogs = document.getElementsByClassName("dialog");
	let clientIds = DeleteAcceptedDialogs(dialogs, serverIds);
	CreateNewDialogs(clientIds, serverIds)
	

}

function CheckIds()
{
	let url = urlBase + "dialog/active";
	let method = "GET";
	sendAjax(url, method, function (result)
	{
		result = JSON.stringify(result);
		result = JSON.parse(result);
		UpdateDialogs(result);
	});
}

setInterval(function(){CheckIds();}, 1000);

const hubConnection = new signalR
	.HubConnectionBuilder()
    .withUrl("/signalr", { transport: signalR.HttpTransportType.WebSockets })
    .build();

hubConnection.on("Delete", function (data) {
	console.log(data);
});

hubConnection.on("Create", function (data) {
	console.log(data);
});

hubConnection.start();