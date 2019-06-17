//callback za klik na klub
function onClubClick(clubId) {
    //ajax request za igrače iz klubova
    $.ajax({
        url: 'https://localhost:5001/api/Nba/Clubs/' + clubId,
        method: 'GET',
        success: function(data) {
            //brisanje prvog reda[podaci nisu učitani]
            $('.row1Player').remove();

            //dohvaćanje tablice
            var $table = $('.playersTablica');
            var $headerRow = $('.headerRow2');

            //sprječavanje ponavljanja rezultata
            $table.html("");
            $table.append($headerRow);

            //iteriranje po rezultatima
            data.players.forEach(function(playeri) { 

                //dodavanje dom elemenata za redove i ćelije
                var $row = $('<tr></tr>');
                var $tdId = $('<td></td>');
                var $tdNumber = $('<td></td>');
                var $tdFirstname = $('<td></td>');
                var $tdLastname = $('<td></td>');
                var $tdGP = $('<td></td>');
                var $tdMIN = $('<td></td>');
                var $tdPTS = $('<td></td>');
                var $tdFGA = $('<td></td>');
                var $tdFGM = $('<td></td>');

                //dodavanje cellova kao djecu row-u
                $row.append($tdId);
                $row.append($tdNumber);
                $row.append($tdFirstname);
                $row.append($tdLastname);
                $row.append($tdGP);
                $row.append($tdMIN);
                $row.append($tdPTS);
                $row.append($tdFGA);
                $row.append($tdFGM);

                //dodavanje sadržaja
                $tdId.text(playeri.id);
                $tdNumber.text(playeri.number);
                $tdFirstname.text(playeri.firstname);
                $tdLastname.text(playeri.lastname);
                $tdGP.text(playeri.gp);
                $tdMIN.text(playeri.min);
                $tdPTS.text(playeri.pts);
                $tdFGA.text(playeri.fga);
                $tdFGM.text(playeri.fgm);

                //dodavanje row-a u tablicu
                $table.append($row);
                
            });
        }
    });
}

$(document).ready(function(){

    //dohvaćanje kluba 
    $.ajax({
        url: "https://localhost:5001/api/Nba/Clubs",
        method: "GET",
        success: function(responseData) {
            //brisanje reda s praznom porukom
            $('.row1Club').remove();

            //dohvaćanje tablice
            var $table = $('.clubsTablica');
            //iteriranje po rezultatima
            responseData.forEach(function(clubs) {
                
                //dodavanje dom elemenata za redove i ćelije
                var $row = $('<tr></tr>');
                var $tdId = $('<td></td>');
                var $tdClub = $('<td></td>');
                var $tdCity = $('<td></td>');
                var $tdGP = $('<td></td>');
                var $tdMIN = $('<td></td>');
                var $tdPTS = $('<td></td>');
                var $tdFGA = $('<td></td>');
                var $tdFGM = $('<td></td>');
                
                //dodavanje cellova kao djecu row-a
                $row.append($tdId);
                $row.append($tdClub);
                $row.append($tdCity);
                $row.append($tdGP);
                $row.append($tdMIN);
                $row.append($tdPTS);
                $row.append($tdFGA);
                $row.append($tdFGM);
                
                //dodavanje sadržaja
                $tdId.text(clubs.id);
                $tdClub.text(clubs.clubName);
                $tdCity.text(clubs.clubCity);
                $tdGP.text(clubs.gp);
                $tdMIN.text(clubs.min);
                $tdPTS.text(clubs.pts);
                $tdFGA.text(clubs.fga);
                $tdFGM.text(clubs.fgm);

                //dodavanje row-a u tablicu
                $table.append($row);

                $row.on('click', function() {
                    onClubClick(clubs.id);
                });
            });
        },
        error: function() {
            alert('greška!');
        }
    });

});