const urlBase = "http://localhost:5001/"


function dialogClick(e)
{
	var element = this;
	var header = element.getElementsByClassName("dialog-header")[0];
	var id = element.getAttribute("dialogid");
	let messages = element.getElementsByClassName("dialog-message");
	let lastMessage = messages[messages.length - 1].getElementsByTagName("p")[0];
	
	if(header.classList.contains("dialog-done"))
	{
		console.log(this)
		element.removeEventListener('click', dialogClick);
		element.remove();
		return;
	}

	if(e.target.className == "npc-submit-btn")
	{
		var sel = element.getElementsByClassName("npc-select")[0];
		var text = sel.options[sel.selectedIndex].text;
		CallSetAnswer(id, text, header, lastMessage);
		return;
	}
	else if(e.target.className == "sendanswer-btn")
	{
		var text = element.getElementsByClassName("textinput-dialog")[0].value;
		CallSetAnswer(id, text, header, lastMessage);
		return;
	}
	else if(e.target.className == "sendtogpt-btn")
	{
		var text = element.getElementsByClassName("textinput-dialog")[0].value;
		CallSendToGpt(id, header, lastMessage);
		return;
	}
	else if(header.classList.contains("dialog-active") || header.classList.contains("dialog-answered"))
	{
		return;
	}
    

	header.className = "dialog-header";
	CallWaitMore(header, id, lastMessage);
}

function sendAjax(url, method, callback, body)
{
	$.ajax({
 
                url: url,
 
                // Type of Request
                type: method,

                data: body,
 
                // Function to call when to
                // request is ok
                success: function (data) {
                    callback(data);
                },
 
                // Error handling
                error: function (error) {
                    console.log(error);
                }
            });
}

function CallWaitMore(header, dialogId, lastMessage)
{
	var url = urlBase + "dialog/getMoreTime/" + dialogId;
	var method = "GET";

	sendAjax(url, method, function(result) {
		result = JSON.stringify(result);
		if(result == "$WAIT_MORE" || result == "\"$WAIT_MORE\"")
		{
			header.className = "dialog-header dialog-active";
			lastMessage.innerText = "[КЛИЕНТ В РЕЖИМЕ ОЖИДАНИЯ]";
		}
		else
		{
			console.log("ERROR! WAIT MORE returned: " + result);
		}
	});
}

function CallSetAnswer(dialogId, message, header, lastMessage)
{
	var url = urlBase + "dialog/setAnswer";
	var method = "POST";
	var body = {
		"dialogId": dialogId,
		"message": message
	}
	sendAjax(url, method, function() {
		header.className = "dialog-header dialog-answered";
		lastMessage.innerText = "$SERVER_ANSWER: " + message;
	}, body);
}

function CallSendToGpt(dialogId, header, lastMessage)
{
	var url = urlBase + "dialog/useGpt/" + dialogId;
	var method = "GET";

	sendAjax(url, method, function(result) {
		header.className = "dialog-header dialog-answered";
		lastMessage.innerText = "$GPT_ANSWER: " + result;

	});
}


function GetMoreTimeOnClick(element)
{
	element.addEventListener('click', dialogClick);
}



document.body.addEventListener('click', function(e) {

  if(e.target.className == "endofgame-btn")
  {
  	var element = e.target;
  	var parent = element.parentNode.parentNode;
  	var textInput = parent.getElementsByClassName("textinput-dialog")[0];
  	textInput.value += "$END_OF_GAME";
  }

});


var statusSpan = document.getElementById("server-status");
var onBtn = document.getElementById("manual-on-btn");
var offBtn = document.getElementById("manual-off-btn");

onBtn.addEventListener('click', function(e) {
	let url = urlBase + "server/changemode?mode=true";
	let method = "GET";
	sendAjax(url, method, function() {
		statusSpan.innerText = "Включен";
	})
});

offBtn.addEventListener('click', function(e) {
	let url = urlBase + "server/changemode?mode=false";
	let method = "GET";
	sendAjax(url, method, function() {
		statusSpan.innerText = "Выключен";
	})
});









// Useless stuff
function SetScrollBottom()
{
	var dialogsMessages = document.getElementsByClassName("dialog-messages");
	for(var i = 0; i < dialogsMessages.length; i++)
	{
		dialogsMessages[i].scrollTop = 100;
	}
}
SetScrollBottom();

elements = document.getElementsByClassName("dialog");
for(var i = 0; i < elements.length; i++)
{
	GetMoreTimeOnClick(elements[i]);
}
