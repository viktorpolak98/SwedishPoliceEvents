const mapForStatisticsboard = new Map();


function clearStatisticsboardMap() {
    mapForStatisticsboard.clear();
}


export function addTypeToStatisticsboardMap(type) {
    if (mapForStatisticsboard.has(type)) {
        mapForStatisticsboard.set(type, mapForStatisticsboard.get(type) + 1);
        return;
    }

    mapForStatisticsboard.set(type, 1);
}

export function addItemsToStatisticsboardAndSort() {

    const tableRef = document.getElementById("statistics").getElementsByTagName("tbody")[0];

    mapForStatisticsboard.forEach((value, key) => {
        
        const newRow = tableRef.insertRow(-1);
        
        const typeCell = newRow.insertCell(0);
        const countCell = newRow.insertCell(1);

        const typeText = document.createTextNode(key);
        typeCell.appendChild(typeText);

        const countText = document.createTextNode(value);
        countCell.appendChild(countText);
    });

    insertionSortTableDescending();
}

export function clearStatisticsboard() {
    clearStatisticsboardMap();
    const tableRef = document.getElementById("statistics").getElementsByTagName("tbody")[0];

    while (tableRef.rows.length > 0) {
        tableRef.deleteRow(0);
    }
}

//Sorts statistics table. Since the contents of statistics is very small the time complexity won't make a difference.
//Therefor an algorithm with lower space complexity is preferred

function insertionSortTableDescending() {
    let table = document.getElementById("statistics");
    

    for (let i = 2; i < table.rows.length; i++) {

        let moves = 0;
        let j = i - 1;
        while (j > 0 && table.rows[i - moves].getElementsByTagName("TD")[1].innerHTML > table.rows[j].getElementsByTagName("TD")[1].innerHTML) {
            table.rows[j].parentNode.insertBefore(table.rows[i - moves], table.rows[j]);
            j--;
            moves++;
        }
    }
}