/**
 * Import function triggers from their respective submodules:
 *
 * const {onCall} = require("firebase-functions/v2/https");
 * const {onDocumentWritten} = require("firebase-functions/v2/firestore");
 *
 * See a full list of supported triggers at https://firebase.google.com/docs/functions
 */

//const {onRequest} = require("firebase-functions/v2/https");
//const logger = require("firebase-functions/logger");

const functions = require("firebase-functions");

exports.gachaDrawWithGuarantees = functions.https.onCall((data, context) => {
    const payload = data.data || data;

    const gradeRanges = payload.gradeRanges;
    const drawCount = Number(payload.drawCount) || 10;

    console.log("🔥 gradeRanges:", gradeRanges);
    console.log("🔥 count:", drawCount);

    const weights = [70, 20, 8, 2]; // Normal, Rare, Epic, Legend
    const guaranteeEpicAt = 10;
    const guaranteeLegendAt = 100;

    if (!gradeRanges || gradeRanges.length !== weights.length) {
        throw new functions.https.HttpsError(
            'invalid-argument',
            'gradeRanges의 길이는 고정된 weights와 일치해야 합니다.'
        );
    }

    // 누적 확률 테이블
    const totalWeight = weights.reduce((a, b) => a + b, 0);
    const cumulative = [];
    let acc = 0;
    for (let i = 0; i < weights.length; i++) {
        acc += weights[i];
        cumulative.push({ gradeIndex: i, threshold: acc });
    }

    const drawOne = () => {
        const rand = Math.random() * totalWeight;
        const selected = cumulative.find(e => rand < e.threshold);
        const maxId = gradeRanges[selected.gradeIndex];
        const itemId = Math.floor(Math.random() * maxId);
        return { grade: selected.gradeIndex, id: itemId };
    };

    const results = [];
    let hasEpic = false;
    let hasLegend = false;

    // 일반 뽑기
    for (let i = 0; i < drawCount; i++) {
        const result = drawOne();
        if (result.grade === 2) hasEpic = true;
        if (result.grade === 3) hasLegend = true;
        results.push(result);
    }

    // Epic 보장 (10회 이상이고 에픽 없음)
    if (drawCount >= guaranteeEpicAt && !hasEpic) {
        const maxId = gradeRanges[2];
        const itemId = Math.floor(Math.random() * maxId);
        results.push({
            grade: 2,
            id: itemId,
            guaranteed: true
        });
    }

    // Legend 보장 (100회 이상이고 레전드 없음)
    if (drawCount >= guaranteeLegendAt && !hasLegend) {
        const maxId = gradeRanges[3];
        const itemId = Math.floor(Math.random() * maxId);
        results.push({
            grade: 3,
            id: itemId,
            guaranteed: true
        });
    }

    return { results };
});





// Create and deploy your first functions
// https://firebase.google.com/docs/functions/get-started

// exports.helloWorld = onRequest((request, response) => {
//   logger.info("Hello logs!", {structuredData: true});
//   response.send("Hello from Firebase!");
// });
