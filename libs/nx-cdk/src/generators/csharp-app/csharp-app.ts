import {
  addProjectConfiguration,
  formatFiles,
  getWorkspaceLayout,
  joinPathFragments,
  ProjectConfiguration,
  generateFiles,
  names,
  Tree,
  workspaceRoot,
} from '@nx/devkit';
import * as path from 'path';
import { CsharpAppGeneratorSchema } from './schema';
import { ProjectType } from '@nx/workspace';

export async function csharpAppGenerator(tree: Tree, options: CsharpAppGeneratorSchema) {
  const { appsDir } = getWorkspaceLayout(tree);

  if (!appsDir) {
    throw new Error('Cannot determine apps directory. Is this an Nx workspace?');
  }

  const devKitNames = names(options.appSubDirectory);
  const projectRoot = `${appsDir}/${options.appSubDirectory}`;
  const projectName = devKitNames.name.replace(new RegExp('/', 'g'), '-');

  // Parse namespace for project name
  const namespaceParts = options.namespace.split('.');
  const stackClassName = namespaceParts[namespaceParts.length - 1];

  // Helper to build CDK commands with env and sandbox context, with defaults
  const cdkCommand = (baseCmd: string, options: { defaults?: { env: string; sandbox: string }; includeStack?: boolean } = {}) => {
    const { defaults = { env: 'dev', sandbox: '' }, includeStack = false } = options;
    const envPart = `--context env=\${NX_ENV:-${defaults.env}}`;
    const sandboxPart = defaults.sandbox !== undefined ? `--context sandbox=\${NX_SANDBOX:-${defaults.sandbox}}` : '--context sandbox=${NX_SANDBOX:-}';

    if (includeStack) {
      return `NX_ENV={args.env} NX_SANDBOX={args.sandbox} NX_STACK={args.stack} bash -c 'if [ -n "\${NX_STACK}" ]; then ${baseCmd} \${NX_STACK} ${envPart} ${sandboxPart}; else ${baseCmd} ${envPart} ${sandboxPart}; fi'`;
    }

    return `NX_ENV={args.env} NX_SANDBOX={args.sandbox} bash -c '${baseCmd} ${envPart} ${sandboxPart}'`;
  };

  const projectConfiguration: ProjectConfiguration = {
    root: projectRoot,
    sourceRoot: joinPathFragments(projectRoot, 'src'),
    projectType: ProjectType.Application,
    name: projectName,
    ...(options.implicitDependencies &&
      options.implicitDependencies.length > 0 && {
        implicitDependencies: options.implicitDependencies,
      }),
    targets: {
      build: {
        executor: 'nx:run-commands',
        options: {
          cwd: projectRoot,
          commands: ['dotnet build'],
        },
      },
      diff: {
        executor: 'nx:run-commands',
        options: {
          cwd: projectRoot,
          commands: [cdkCommand('cdk diff')],
        },
      },
      synth: {
        executor: 'nx:run-commands',
        options: {
          cwd: projectRoot,
          commands: [cdkCommand('cdk synth')],
        },
      },
      deploy: {
        executor: 'nx:run-commands',
        options: {
          cwd: projectRoot,
          commands: [cdkCommand('cdk deploy --require-approval=never')],
        },
      },
      destroy: {
        executor: 'nx:run-commands',
        dependsOn: ['destroy^'],
        options: {
          cwd: projectRoot,
          commands: [cdkCommand('cdk destroy -f', { includeStack: true })],
        },
      },
      test: {
        executor: 'nx:run-commands',
        options: {
          cwd: projectRoot,
          commands: ['dotnet test'],
        },
      },
    },
    tags: ['cdk', 'csharp', 'type:infrastructure', 'lang:csharp'],
  };

  generateFiles(tree, path.join(__dirname, 'files'), projectRoot, {
    ...options,
    ...devKitNames,
    projectName,
    stackClassName,
    templ: '', // to remove the __templ__ in the file names
    hasDependencies: options.implicitDependencies && options.implicitDependencies.length > 0,
    dependencyNames: options.implicitDependencies || [],
  });

  addProjectConfiguration(tree, projectName, projectConfiguration);

  await formatFiles(tree);
}

export default csharpAppGenerator;
