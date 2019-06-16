$(document).ready(function(){

    //getanje klubova
    $.ajax({
        url: "https://localhost:5001/api/Nba/Clubs",
        method: "GET",
        success: function(responseData) {
            responseData.forEach(function(club){

                
                var value = club.id;
                var $options = $("<option>",{
                    value: value
                });
                var $select = $(".idKluba");


                $select.append($options);

                $options.text(club.clubCity + ' ' + club.clubName);
                
            });
        }
    });

    //postanje kluba
    $(".clubForm").submit(function(event){
        event.preventDefault();
        event.stopPropagation();
    });

    $('input[name="Submit1"]').on('click', function(event){
        event.preventDefault();
        event.stopPropagation();

        var clubName = $("input[name='Clubname']").val();
        var clubCity = $("input[name='Clubcity']").val();
        var GP = $("input[name='GP']").val();
        var PTS = $("input[name='PTS']").val();
        var MIN = $("input[name='MIN']").val();
        var FGM = $("input[name='FGM']").val();
        var FGA = $("input[name='FGA']").val();

        //objekt za klub    
        var clubObject = {
            clubName: clubName,
            clubCity: clubCity,
            gp: GP,       
            min: PTS,
            pts: MIN,
            fgm: FGM,
            fga: FGA
        };

        $.ajax({
            url: "https://localhost:5001/api/Nba/Clubs",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(clubObject),
            success: function(responseData) {
                alert('USPJEH!');

                var value = responseData.id;
                var $options = $("<option>",{
                    value: value
                });
                var $select = $(".idKluba");


                $select.append($options);

                $options.text(responseData.clubCity + ' ' + responseData.clubName);
                
                
            },
            error: function(responseData){
                alert('GREŠKA!!!');
                console.log(responseData);
            }            
        });
    });


    //postanje igrača
    $(".playerForm").submit(function(event){
        event.preventDefault();
        event.stopPropagation();
    });

    $('input[name="Submit2"]').on('click', function(event){
        event.preventDefault();
        event.stopPropagation();

        var number = $("input[name='Number']").val();
        var firstname = $("input[name='Firstname']").val();
        var lastname = $("input[name='Lastname']").val();
        var GP = $("input[name='GPp']").val();
        var MIN = $("input[name='MINp']").val();
        var PTS = $("input[name='PTSp']").val();
        var FGM = $("input[name='FGMp']").val();
        var FGA = $("input[name='FGAp']").val();
        var clubId = $(".idKluba").val();

        var playerObject = {
            number: number,
            firstname: firstname,
            lastname: lastname,
            gp: GP,
            min: MIN,
            pts: PTS,
            fgm: FGM,
            fga: FGA,
            clubId: clubId
        };

        $.ajax({
            url: 'https://localhost:5001/api/Nba/Players',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(playerObject),
            success: function(responseData){
                alert('USPJEH!');
                console.log(responseData);
            },
            error: function(responseData){
                alert('GREŠKA!!!');
                console.log(responseData);
            }
        });
    });
});