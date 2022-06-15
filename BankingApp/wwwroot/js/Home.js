

//Below is the method to add a new account
//We use AJAX, and an asynchronous POST request to send the selected option from sweet alert
//We then use the OnPostAddNewAccount method that can be found in our Home.cshtml.cs page
$("#newacnt").click(async() => 
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
        input: "select",
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
            beforeSend: (xhr) =>
            {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(acntType),
            success: () =>
            {
                Swal.fire
                ({
                    title: 'Account Created Succesfully',
                    text: '',
                    icon: 'success',
                })
                .then(() => //Swal.fire.then
                {
                    window.location.reload();//reload to get database changes to the page
                })
            },
            error: () => 
            {
                Swal.fire
                    ({
                        title: "error",
                        text: "oops, something went wrong",
                        icon: "error"
                    })
            }//end error

        })//end ajax

    }//end if(acntType)

});//end newacnt function




//transfer funds
$("#transfer").click( async() =>
{
    var Options;
    for (let x in TransferInputOptions, TransferInputOptionsIDs)
    {
        var temp = TransferInputOptions[x];
        var ID = TransferInputOptionsIDs[x];
        Options += `<option name = ${ID} value=${temp}>${temp}- Acnt Number:${ID}</option>`
    }
    var form =
    `
        <div class="form-floating mx-auto mb-4">
            <select class="form-select" id="swal-input-from" name="s1">
                <option selected>Select an account</option>
                ${Options}
                <span> </span>
            </select>
            <label for="#st1"> From: </label>
        </div>
        <div class=" form-floating mx-auto mt-4 mb-4">
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
    `;

    const {value: Transferdata } =
    await new Swal
    ({
        title: "Transfer Funds:",
        html: form,
        focusConfirm: false,
        preConfirm: () =>
        {
            var error = false;
            //var ID = document.getElementById('swal-input-from').attributes("name")
            var fromID = $('#swal-input-from').find(":selected").attr("name");
            var toID = $('#swal-input-to').find(":selected").attr("name");
            
            var from = document.getElementById('swal-input-from').value;
            var to = document.getElementById('swal-input-to').value;
            var dollars = document.getElementById('swal-input-dollars').value;
            var cents = document.getElementById('swal-input-cents').value;

            if (from == "Select an account" || to == "Select an account")
            {
                swal.close();
                return false;
            }

            if (dollars == "")
            {
                swal.close();
                return false;
            }

            if (cents == "")
            {
                cents = 0;
            }

            var response =
            {
                from : from.toString(),
                to : to.toString(),
                dollars : dollars.toString(),
                cents: cents.toString(),
                fromID: fromID.toString(),
                toID: toID.toString()
            };

            return response;
        },
        showCancelButton: true,
        showConfirmButton: true,
    })

    if (Transferdata)
    {
        //add ajax post to server to initiate a transfer
        const URL = './Home?handler=TransferFunds';
        $.ajax
        ({
            type: 'POST',
            url: URL,
            async: true,
            beforeSend:(xhr) =>
            {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(Transferdata),
            showConfirmButton: true,
            showCancelButton: true,
            success: () =>
            {
                Swal.fire
                ({
                    title: 'Funds added Successfully!',
                    html:
                        `
                        <div class="container text-center w-100 mx-auto">                    
                            \$${Transferdata.dollars}.${Transferdata.cents} was transferred <br/> 
                            from Account: ${Transferdata.fromID} <br/>
                            to Account: ${Transferdata.toID}
                        </div>
                        `,
                    icon: 'success'
                })
                .then(() => //Swal.fire.then
                {
                    window.location.reload();//reload to get database changes to the page    
                })
            },
            error:(jqXHR, textStatus, errorThrown) =>//errors if needed
            {
                Swal.fire
                    ({
                        title: "Error initiating transfer",
                        text: jqXHR.responseText,
                        icon: "error"
                    })
            }
        })//end ajax
    }
        
})


//add funds
addfunds = async (id) =>
{
    const { value: funds } =
    await new Swal
    ({
        title: "Add funds:",
        text: id,
        html:
            `
                <div class="input-group w-50 mx-auto mt-4">
                    <span class="input-group-text">Dollars </span>
                    <input id="swal-input-dollars1" type="number" min="0" step="1" onkeypress="return event.charCode >= 48 && event.charCode <= 57" class="form-control"/>
                </div>
                <div class="input-group w-50 mx-auto mt-4 mb-2">
                    <span class="input-group-text">&nbsp;Cents&nbsp;</span>
                    <input id="swal-input-cents1" type="number" max="99" pattern="[0-99]" step="1" onkeypress="return event.charCode >= 48 && event.charCode <= 57" class="form-control"/>
                </div>
            `,
        preConfirm: () =>
        {
            var dollars = $('#swal-input-dollars1').val();
            var cents = $('#swal-input-cents1').val();

            if (dollars == "")
            {
                return false;
                Swal.close();
            }

            if (cents == "") 
                cents = 0
                
            var data =
            {
                dollars: dollars.toString(),
                cents: cents.toString(),
                ID: id.toString()
            };
            return data;
        },
        showConfirmButton: true,
        showCancelButton: true,
    })

    if (funds)
    {
        const URL = './Home?handler=AddFunds';
        $.ajax
        ({
            type: 'POST',
            url: URL,
            async: true,
            beforeSend: (xhr) =>
            {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(funds),
            showConfirmButton: true,
            showCancelButton: true,
            success: () =>
            {
                Swal.fire
                    ({
                        title: 'Funds added Successfully!',
                        html:
                        `
                            <div class="container text-center w-100 mx-auto">                    
                                \$${funds.dollars}.${funds.cents} was deposited into Account: ${funds.ID}                                       
                            </div>
                        `,
                        icon: 'success'
                    })
                    .then(() => //Swal.fire.then
                    {
                        window.location.reload();//reload to get database changes to the page    
                    })
            },
            error: (jqXHR, textStatus, errorThrown) =>//errors if needed
            {
                Swal.fire
                ({
                    title: "error",
                    text: "oops, something went wrong",
                    icon: "error"
                })
            }
        })//end ajax
    }//end if
};//end addfunds



