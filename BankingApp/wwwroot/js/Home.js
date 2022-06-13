

        //Below is the method to add a new account
        //We use AJAX, and an asynchronous POST request to send the selected option from sweet alert
        //We then use the OnPostAddNewAccount method that can be found in our Home.cshtml.cs page
        $("#newacnt").click(() => 
        {
            console.log("in newaccount function");
            (async () => 
            {
                const inputOptions =
                {
                    'Savings': 'Savings',
                    'Investment': 'Investment',
                    'Checking': 'Checking'
                }

                const { value: acntType } = 
                await new Swal
                ({
                    title: "Which type of account?",
                    input: "radio",
                    showCancelButton: true,
                    showConfirmButton: true,
                    inputOptions: inputOptions,
                    inputValidator: (value) => 
                    {
                        if (!value) 
                        {
                            return "You must select an account";
                        }//end if(!value)

                    }//end input validator

                })//end await Swal.fire


                if (acntType) 
                {
                    const URL = './Home?handler=AddNewAccount';

                    $.ajax
                    ({
                        type: 'POST',
                        url: URL,
                        async: true,
                        beforeSend: function(xhr) 
                        {
                            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: JSON.stringify(acntType),
                        success: function(data) 
                        {
                            location.reload();
                        },
                        error: function(error) 
                        {
                            console.log(`Error ${error}`);
                        }//end error

                    })//end ajax
                    .then//ajax then
                    (
                        Swal.fire
                        (
                            'Account Created Succesfully',
                            '',
                            'success',

                        )
                        .then((result) => //Swal.fire.then
                        {
                            if (result.isConfirmed) 
                            {
                                window.location.reload();//reload to get database changes to the page

                            }//end if(result.isConfirmed)

                        })//end swal fire

                    )//end ajax.then

                }//end if(acntType)

            })();//end async

        });//end function

    $("#transfer").click( async function()
    {
        var numOptions = 1;
        var Options;
        for (let x in TransferInputOptions)
        {
            var temp = TransferInputOptions[x];
            Options += `<option value=${temp}>${temp}</option>`
            numOptions++;
        }

        $()

        var form =
        `
        <div class="container w-100">
            <div class="form-floating mx-auto mb-2">
                <select class="form-select" id="swal-input-from" name="s1">
                    <option selected>Select an account</option>
                    ${Options}
                </select>
                <label for="#st1"> From: </label>
            </div>
            <div class=" form-floating mx-auto mt-2 mb-2">
                <select class="form-select" id="swal-input-to" name="s2">
                    <option selected>Select an account</option>
                    ${Options}
                </select>
                <label for="#s2"> To: </label>
            </div>
            <div class="input-group w-50 mx-auto">
                <span class="input-group-text">Dollars </span>
                <input id="swal-input-dollars" type="number" min="0" step="1" onkeypress="return event.charCode >= 48 && event.charCode <= 57" class="form-control">
            </div>
            <div class="input-group w-50 mx-auto mt-2 mb-2">
                <span class="input-group-text">&nbsp;Cents&nbsp;</span>
                <input id="swal-input-cents" type="number" max="99" pattern="[0-99]" step="1" onkeypress="return event.charCode >= 48 && event.charCode <= 57" class="form-control">
            </div>
        </div>
        `;



        const {value: Transferdata } =
        await new Swal
        ({
            title: "Transfer Funds:",
            html: form,
            focusConfirm: false,
            preConfirm: () => //can only transfer if there are 2 accounts
            {
                var error = false;
                var from = document.getElementById('swal-input-from').value;
                var to = document.getElementById('swal-input-to').value;
                var dollars = document.getElementById('swal-input-dollars').value;
                var cents = document.getElementById('swal-input-cents').value;

                if (from == "Select an account" || to == "Select an account")
                {
                    swal.close();
                    return false;

                }
                if (dollars == "" || cents == "")
                {
                    swal.close();
                    return false;

                }
                var response =
                {
                    "from" : from,
                    "to" : to,
                    "dollars" : dollars,
                    "cents" : cents
                };

                return response;
            },
            showCancelButton: true,
            showConfirmButton: true,
        })

        //if we get here... 
        if (Transferdata)
        {
            await Swal.fire
            ({
                html:
                `
                    from account: ${Transferdata.from} <br/>
                    to account: ${Transferdata.to} <br/>
                    amount: \$${Transferdata.dollars}.${Transferdata.cents} <br/>
                `
            });
        }
        
    })