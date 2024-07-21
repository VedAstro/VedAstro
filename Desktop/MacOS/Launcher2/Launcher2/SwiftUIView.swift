//import SwiftUI
//
//struct Form1: View {
//    @State private var process: Process?
//
//    var body: some View {
//        VStack {
//            Button("Relaunch") {
//                relaunchButtonClicked()
//            }
//            TextEditor(text: $outputText)
//               .frame(maxWidth:.infinity, maxHeight:.infinity)
//        }
//       .onAppear {
//            startAzureFunctionsCli()
//        }
//       .onDisappear {
//            killProcess()
//        }
//    }
//
//    @State private var outputText = ""
//
//    private func relaunchButtonClicked() {
//        startAzureFunctionsCli()
//    }
//
//    private func startAzureFunctionsCli() {
//        let apiBuildPath = URL(fileURLWithPath: Bundle.main.resourcePath!).appendingPathComponent("api-build")
//        let funcExecPath = URL(fileURLWithPath: Bundle.main.resourcePath!).appendingPathComponent("Azure.Functions.Cli", "func.exe")
//
//        let process = Process()
//        process.currentDirectoryPath = apiBuildPath.path
//        process.launchPath = funcExecPath.path
//        process.arguments = ["start"]
//        process.terminationHandler = { _ in
//            self.process = nil
//        }
//
//        let outputPipe = Pipe()
//        let errorPipe = Pipe()
//        process.standardOutput = outputPipe
//        process.standardError = errorPipe
//
//        outputPipe.fileHandleForReading.waitForDataInBackgroundAndNotify()
//        errorPipe.fileHandleForReading.waitForDataInBackgroundAndNotify()
//
//        process.launch()
//
//        self.process = process
//
////        NotificationCenter.default.addObserver(forName:.NSFileHandleReadCompletionNotification, object: outputPipe.fileHandleForReading, queue: nil) { notification in
////            if let data = outputPipe.fileHandleForReading.availableData {
////                let output = String(data: data, encoding:.utf8)?? ""
////                self.outputText += output
////            }
////            outputPipe.fileHandleForReading.waitForDataInBackgroundAndNotify()
////        }
////
////        NotificationCenter.default.addObserver(forName:.NSFileHandleReadCompletionNotification, object: errorPipe.fileHandleForReading, queue: nil) { notification in
////            if let data = errorPipe.fileHandleForReading.availableData {
////                let error = String(data: data, encoding:.utf8)?? ""
////                self.outputText += error
////            }
////            errorPipe.fileHandleForReading.waitForDataInBackgroundAndNotify()
////        }
//    }
//
//    private func killProcess() {
//        process?.terminate()
//    }
//}
//
//struct Form1_Previews: PreviewProvider {
//    static var previews: some View {
//        Form1()
//    }
//}
