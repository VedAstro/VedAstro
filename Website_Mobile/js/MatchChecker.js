updateHistory();

new PageHeader("PageHeader");

new IconButton("IconButton_Calculate");
//new InfoBox("InfoBox_FindMatch");
new InfoBox("InfoBox_FullCheck");

var personSelectorMale = new PersonSelectorBox("PersonSelectorBox_Male");
var personSelectorFemale = new PersonSelectorBox("PersonSelectorBox_Female");



async function OnClickCalculateMatch() {

    //check if both persons selected
    let selectedMale = await personSelectorMale.GetSelectedPerson();
    let selectedFemale = await personSelectorFemale.GetSelectedPerson();

    //if no selected person then ask user if sleeping 😴
    if (selectedMale == null || selectedFemale == null) {
        Swal.fire({ icon: 'warning', title: 'Are you sleeping? 😴', html: 'Please select 2 person names to calculate match.', showConfirmButton: true });
        return; //end here
    }

    //--------------------------------------- LETS START -------------------------------------

    //show loading to user
    CommonTools.ShowLoading();

    //put person name into tab title for easy multi-tabbing (UX ease)
    document.title = `${selectedMale.DisplayName} + ${selectedFemale.DisplayName}`;

    //generate full match report from API
    let matchReport = await getMatchReportFromAPI(selectedMale.BirthTime.ToUrl(), selectedFemale.BirthTime.ToUrl());

    //convert "match prediction list" to HTML and inject into page
    var advancedPredictionsTableHtml = convertPredictionListToHtml(matchReport.PredictionList);
    $("#advancedMatchReportHolder").empty();
    $("#advancedMatchReportHolder").html(advancedPredictionsTableHtml);

    var easyMatchReportHtml = generateEasyMatchReportHtml(selectedMale.Name, selectedFemale.Name, matchReport);
    $("#easyMatchReportHolder").empty();
    $("#easyMatchReportHolder").html(easyMatchReportHtml);

    //hide sidebar links for nice clean fit (UX improvement)
    await $("#mainOutputHolder").slideDown(2000);

    //play sound for better UX
    playBakingDoneSound();

    //hide loading
    Swal.close();

    //move view to show report (delay NEEDED else slide & other animations override it)
    setTimeout(() => { scrollToDivById('mainOutputHolder'); }, 500);
}

function generateEasyMatchReportHtml(maleName, femaleName, matchReportJson) {
    return `
        <div class="container mb-4 border rounded-3">
            <div class="row justify-content-start">
                <div class="col-2 d-flex align-items-center">
                    Couple
                </div>
                <div class="col-4 w-auto"><!-- auto width needed, else names get squeezed -->
                    <h4 class="hstack gap-3">
                        ${maleName} <iconify-icon icon="${matchReportJson.Summary.HeartIcon}" width="37" height="37" ></iconify-icon> ${femaleName}
                    </h4>
                </div>
            </div>
            <div class="row justify-content-start">
                <div class="col-2 d-flex align-items-center">
                    Score
                </div>
                <div class="col-4">
                    <b style="color:${matchReportJson.Summary.ScoreColor}; font-size: 33px;">${matchReportJson.KutaScore}%</b>
                </div>
            </div>
            <div class="row justify-content-start">
                <div class="col-2 d-flex align-items-center">
                    Summary
                </div>
                <div class="col-4 w-auto">
                    <h4>
                       ${matchReportJson.Summary.ScoreSummary}
                    </h4>
                </div>
            </div>
        </div>


    `;
}

//given a list of kuta predictions (array), converts to HTML table
function convertPredictionListToHtml(predictionListJson) {
    return `
    <table class="table table-striped table-hover table-bordered shadow" style="max-width:764px; border-radius: 10px; overflow: hidden;"> <!--overflow hidden to make border rounded work-->
      <thead class="table-dark">
        <tr>
          <th class="col-4"><span class="hstack gap-2">Name</span></th>
          <th class="col">
            <span class="hstack gap-2">
              Result
            </span>
          </th>
        </tr>
      </thead>
      <tbody>
        ${predictionListJson.filter(prediction => prediction.Name !== "Empty").map(prediction => `
          <tr>
            <td scope="row" style="width: 412px;">
              <b style="font-size: 22px;">${prediction.Name}</b>
              <p style="margin: 0px; font-size: 15px;">${prediction.Description}</p>
            </td>
            <td class="d-flex justify-content-between">
              <div>
                <b style="color: ${(prediction.Nature === "Good") ? "green" : (prediction.Nature === "Bad") ? "red" : "gray"}; font-size: 22px;">${prediction.Nature}</b>
                <div style="color: #616161; font-size: 14px;">
                  ${prediction.Info}
                  <div class="hstack gap-2">[male: ${prediction.MaleInfo}, female: ${prediction.FemaleInfo}]</div>
                </div>
              </div>
            </td>
          </tr>
        `).join('')}
      </tbody>
    </table>
  `;
}

async function getMatchReportFromAPI(maleBirthTimeUrl, femaleBirthTimeUrl) {

    try {
        const response = await fetch(`${VedAstro.ApiDomain}/Calculate/MatchReport/${maleBirthTimeUrl}${femaleBirthTimeUrl}`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        if (data.Status !== 'Pass') {
            throw new Error('Failed to retrieve data. Status is not "Pass".');
        }

        // return data as is
        return data.Payload.MatchReport;

    } catch (error) {
        console.error('Error fetching data:', error);
        return null;
    }
}
